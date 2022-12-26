namespace Optimum.Messaging.Kafka.Contracts
{
   public interface IKafkaConsumer
    {
        void ConsumeWithResult<TMessage>(string serviceId, Action<KafkaMessageContext<TMessage>> action,CancellationToken token);
    }
}
