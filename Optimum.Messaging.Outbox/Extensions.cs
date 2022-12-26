using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.Outbox;
using Optimum.Messaging.Outbox.Contracts;
using Optimum.Messaging.Outbox.HostedServices;

namespace Optimum.Messaging.RabbitMQ;

public static class Extensions
{
    public static void UseInMemoryOutbox(this IMessageBrokerConfigure configure)
    {
        configure.Builder.Services.AddSingleton<IDataAccessor, InMemoryDataAccessor>();
        configure.Builder.Services.AddTransient<IOutboxStore, OutboxStore>();

        configure.Builder.Services.AddHostedService<OutboxHostedService>();
    }
}