using System.Net;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.Kafka.Contracts;
using Optimum.Messaging.Kafka.HostedServices;
using Optimum.Messaging.Kafka.Options;

namespace Optimum.Messaging.Kafka;

public static class Extensions
{
    public static void UseKafka(this IMessageBrokerConfigure configure)
    {
        var configuration = configure.Builder.Services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();

        var kafkaOptions = configuration.GetSection("MessageBroker").GetSection("Kafka").Get<KafkaOptions>();

        if (kafkaOptions == null)
        {
            throw new InvalidDataException("Please Insert 'Kafka' Section in 'appsettings.json' file.");
        }

        configure.Builder.Services.AddSingleton(kafkaOptions);


        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaOptions.URI,
            ClientId = Dns.GetHostName(),
        };
        
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.URI,
            GroupId = configure.Builder.Options.Id,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        

        configure.Builder.Services.AddSingleton(producerConfig);
        configure.Builder.Services.AddSingleton(consumerConfig);

        configure.Builder.Services.AddTransient<IKafkaConsumer, KafkaConsumer>();
        // configure.Builder.Services.AddTransient<IMessageBus, RabbitMqMessageBus>();
        
        configure.Builder.Services.AddHostedService<ConsumerHostedService>();
    }
}