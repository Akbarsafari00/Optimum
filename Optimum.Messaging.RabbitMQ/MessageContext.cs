using System.Text;
using Newtonsoft.Json;
using Optimum.Messaging;
using RabbitMQ.Client;

namespace Optimum.Messaging.RabbitMQ;

public class RMQMessageContext<T> : MessageContext<T>
{
    private IModel _channel;


    public RMQMessageContext(IModel model, T message, IBasicProperties basicProperties) : base(message,
        basicProperties.CorrelationId)
    {
        _channel = model;
        BasicProperties = basicProperties;
    }

    public IBasicProperties BasicProperties { get; set; }


    public override async Task RespondeAsync<TTRespond>(TTRespond message)
    {

        if (BasicProperties.ReplyTo==null)
        {
            return;
        }
        
        var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        var replyProps = _channel.CreateBasicProperties();
        replyProps.CorrelationId = BasicProperties.CorrelationId;

        _channel.BasicPublish(exchange: "Optimum.EXC",
            routingKey: BasicProperties.ReplyTo,
            mandatory: true,
            basicProperties: replyProps,
            body: response);
    }
}