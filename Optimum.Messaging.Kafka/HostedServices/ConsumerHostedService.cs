using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optimum.Messaging.Attributes;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.Kafka.Contracts;
using Optimum.Options;
using Optimum.Utilitis;

namespace Optimum.Messaging.Kafka.HostedServices
{
    public class ConsumerHostedService : IHostedService
    {
        ServiceOptions _serviceOptions;
        IServiceProvider _serviceProvider;
        private IKafkaConsumer _kafkaConsumer;
        public ConsumerHostedService(IServiceProvider serviceProvider,
            ServiceOptions serviceOptions, IKafkaConsumer kafkaConsumer)
        {
            _serviceProvider = serviceProvider;
            _serviceOptions = serviceOptions;
            _kafkaConsumer = kafkaConsumer;
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

                var method = _kafkaConsumer.GetType().GetMethod("ConsumeWithResult")
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

                method?.Invoke(_kafkaConsumer, new object[]
                {
                    serviceId,
                    action,
                    cancellationToken
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}