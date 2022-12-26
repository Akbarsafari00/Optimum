namespace Optimum.Messaging.Contracts;

public interface IMessageContext<T>
{
    public string CorrelationId { get; set; }
    public T Message { get; set; }
    public Task RespondeAsync<TTRespond>(TTRespond message);


}