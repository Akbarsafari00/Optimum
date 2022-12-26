using Optimum.Contracts;

namespace Optimum.Messaging.Contracts;

public interface IMessageBrokerConfigure
{
    public List<Type> ConsumerTypes { get; set; }
    IOptimumBuilder Builder { get; }


}