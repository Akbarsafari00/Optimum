namespace Optimum.Messaging.Contracts;

public interface IMessageConsumer<TMessage>
{
    Task HandleAsync(MessageContext<TMessage> t);
}