namespace Optimum.Messaging.RabbitMQ.Contracts
{
   public interface IRabbitMqConsumer
    {
        void Consume<T>(string serviceId,Action<T> action);
        void ConsumeWithResult<TMessage>(string serviceId, Action<RMQMessageContext<TMessage>> action);
        Task<T> ConsumeReply<T>(string serviceId) ;
    }
}
