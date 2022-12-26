using Optimum.Messaging.Contracts;

namespace Optimum.Messaging;

public abstract class MessageContext<T> : IMessageContext<T>
{
    
    public MessageContext( T message, string correlationId)
    {
        Message = message;
        CorrelationId = correlationId;
    }

    public string CorrelationId { get; set; }
    public T Message { get; set; }

    public abstract  Task RespondeAsync<TTRespond>(TTRespond message);



}