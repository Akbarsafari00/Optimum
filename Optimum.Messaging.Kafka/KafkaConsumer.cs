using Confluent.Kafka;
using Newtonsoft.Json;
using Optimum.Messaging.Kafka.Contracts;

namespace Optimum.Messaging.Kafka;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly ConsumerConfig _consumerConfig;


    public KafkaConsumer(ConsumerConfig consumerConfig)
    {
        _consumerConfig = consumerConfig;
    }

    public void ConsumeWithResult<TMessage>(string serviceId, Action<KafkaMessageContext<TMessage>> action,CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        
        consumer.Subscribe(serviceId);

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(cancellationToken);

            var o = JsonConvert.DeserializeObject<TMessage>(consumeResult.Message.Value);
            if (o == null) continue;
                
            var ctx = new KafkaMessageContext<TMessage>(o);
            action.Invoke(ctx);
        }

        consumer.Close();
    }
}