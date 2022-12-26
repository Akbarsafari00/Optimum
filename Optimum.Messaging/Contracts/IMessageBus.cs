namespace Optimum.Messaging.Contracts;

public interface IMessageBus
{
    Task SendAsync<T>(string serviceId,T message);
    Task PublishAsync<T>(T @event);
    Task<TResponse?> RequestAsync<TMessage, TResponse>(string serviceId, TMessage message,int timeout = 5000);
}