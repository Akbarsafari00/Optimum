namespace Optimum.Messaging.Contracts;

public interface IMessagePublisher
{
    Task SendAsync<T>(string serviceId,T message);
    Task SendAsync(string serviceId,string name , object message);
    Task PublishAsync<T>(T @event);
    Task PublishAsync(string name, object message);
    Task<TResponse?> RequestAsync<TMessage, TResponse>(string serviceId, TMessage message,int timeout = 5000);
    Task<TResponse?> RequestAsync<TResponse>(string serviceId, string name, object message, int timeout = 5000);
}