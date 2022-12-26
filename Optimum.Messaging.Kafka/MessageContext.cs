using Optimum.Messaging;

namespace Optimum.Messaging.Kafka;

public class KafkaMessageContext<T> : MessageContext<T>
{


    public KafkaMessageContext(T message) : base(message,
        Guid.NewGuid().ToString())
    {
    }

    public override async Task RespondeAsync<TTRespond>(TTRespond message)
    {

        // if (BasicProperties.ReplyTo==null)
        // {
        //     return;
        // }
        //
        // var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        //
        // var replyProps = _channel.CreateBasicProperties();
        // replyProps.CorrelationId = BasicProperties.CorrelationId;
        //
        // _channel.BasicPublish(exchange: "",
        //     routingKey: BasicProperties.ReplyTo,
        //     mandatory: true,
        //     basicProperties: replyProps,
        //     body: response);
    }
}