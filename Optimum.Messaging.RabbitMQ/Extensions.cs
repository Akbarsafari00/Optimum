using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.RabbitMQ.Contracts;
using Optimum.Messaging.RabbitMQ.HostedServices;
using Optimum.Messaging.RabbitMQ.Options;
using RabbitMQ.Client;

namespace Optimum.Messaging.RabbitMQ;

public static class Extensions
{
    public static void UseRabbitMq(this IMessageBrokerConfigure configure)
    {
        var configuration = configure.Builder.Services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();

        var rmqOptions = configuration.GetSection("MessageBroker").GetSection("RabbitMq").Get<RabbitMqOptions>();

        if (rmqOptions == null)
        {
            throw new InvalidDataException("Please Insert 'RabbitMq' Section in 'appsettings.json' file.");
        }

        configure.Builder.Services.AddSingleton(rmqOptions);


        var factory = new ConnectionFactory()
        {
            Uri = new Uri(rmqOptions.URI),
        };
        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare("Optimum.EXC", ExchangeType.Direct, true, false);

        configure.Builder.Services.AddSingleton(connection);
        configure.Builder.Services.AddSingleton(channel);

        configure.Builder.Services.AddTransient<IRabbitMqConsumer, RabbitMqConsumer>();
        configure.Builder.Services.AddTransient<IMessagePublisher, RabbitMqPublisher>();
        
        configure.Builder.Services.AddHostedService<ConsumerHostedService>();
    }
}