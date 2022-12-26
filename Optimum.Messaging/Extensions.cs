using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;
using Optimum.Messaging.Configurations;
using Optimum.Messaging.Contracts;
using Optimum.Utilitis;

namespace Optimum.Messaging;

public static class Extensions
{
    public static IOptimumBuilder AddMessageBroker(this IOptimumBuilder builder,Action<IMessageBrokerConfigure> configuration)
    {
        configuration(new MessageBrokerConfigure(builder));
        
        var items = TypeUtil.GetAllTypesImplementingOpenGenericType(typeof(IMessageConsumer<>),
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()));

        foreach (var item in items)
        {
            builder.Services.AddTransient(item);
        }

        builder.Services.AddTransient<IMessageBus, MessageBus>();
        
        return builder;
    }

    
}
