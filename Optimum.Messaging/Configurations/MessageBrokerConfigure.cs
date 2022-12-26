using Optimum.Contracts;
using Optimum.Messaging.Contracts;

namespace Optimum.Messaging.Configurations;

public class MessageBrokerConfigure : IMessageBrokerConfigure
{
    public List<Type> ConsumerTypes { get; set; }
    public IOptimumBuilder Builder { get; }

    public MessageBrokerConfigure(IOptimumBuilder builder)
    {
        Builder = builder;
        ConsumerTypes = new List<Type>();
    }
}