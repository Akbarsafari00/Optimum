using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optimum.Messaging.Attributes;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.RabbitMQ.Contracts;
using Optimum.Options;
using Optimum.Utilitis;

namespace Optimum.Messaging.RabbitMQ.HostedServices
{
    public class ConsumerHostedService : IHostedService
    {
        ServiceOptions _serviceOptions;
        IServiceProvider _serviceProvider;
        private IRabbitMqConsumer _rabbitMqConsumer;

        public ConsumerHostedService(IServiceProvider serviceProvider, IRabbitMqConsumer rabbitMqConsumer,
            ServiceOptions serviceOptions)
        {
            _serviceProvider = serviceProvider;
            _rabbitMqConsumer = rabbitMqConsumer;
            _serviceOptions = serviceOptions;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var items = TypeUtil.GetAllTypesImplementingOpenGenericType(typeof(IMessageConsumer<>),
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()));
            using var scope = _serviceProvider.CreateScope();

            foreach (var item in items)
            {
                var resultGenericType = item.GetInterfaces().First().GetGenericArguments().Last();
                var messageGenericType = item.GetInterfaces().First().GetGenericArguments().First();

                var method = _rabbitMqConsumer.GetType().GetMethod("ConsumeWithResult")
                    ?.MakeGenericMethod(new Type[] { messageGenericType });

                var serviceFind = scope.ServiceProvider.GetRequiredService(item);

                var serviceId = _serviceOptions.Id;

                var attr = item.GetCustomAttribute<MessageConsumerAttribute>();

                if (attr != null)
                {
                    serviceId = attr.ServiceId;
                }

                Action<object> action = (data) =>
                {
                    item.GetMethod("HandleAsync")?.Invoke(serviceFind, new object[]
                    {
                        data
                    });
                };

                method?.Invoke(_rabbitMqConsumer, new object[]
                {
                    serviceId,
                    action
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}