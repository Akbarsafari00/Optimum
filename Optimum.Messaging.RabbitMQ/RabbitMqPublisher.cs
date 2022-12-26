using System.Collections.Concurrent;
using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.RabbitMQ.Contracts;
using Optimum.Messaging.RabbitMQ.Options;
using Optimum.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Optimum.Messaging.RabbitMQ;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly ServiceOptions _serviceOptions;

    private IModel _channel;
    BlockingCollection<string> respQueue = new BlockingCollection<string>();
    private readonly IBasicProperties prop;

    public RabbitMqPublisher(ServiceOptions serviceOptions, IModel channel)
    {
        _serviceOptions = serviceOptions;
        _channel = channel;
    

        prop = _channel.CreateBasicProperties();

        prop.CorrelationId = Guid.NewGuid().ToString();

       
    }

    public Task SendAsync<T>(string serviceId, T message)
    {
        return SendAsync(serviceId, typeof(T).Name, message);
    }

    public Task PublishAsync<T>(T @event)
    {
       return PublishAsync(typeof(T).Name , @event);
    }

    public  Task<TResponse?> RequestAsync<TMessage, TResponse>(string serviceId, TMessage message,
        int timeout = 5000)
    {
        return RequestAsync<TResponse>(serviceId, typeof(TMessage).Name, message, timeout);
    }

    public async Task SendAsync(string serviceId, string name, object message)
    {
        await BasicSend($"{serviceId}.{name}", message);

    }
    public async Task PublishAsync(string name, object message)
    {
        await BasicSend($"{_serviceOptions.Id}.{name}", message);
    }

    public async Task<TResponse?> RequestAsync<TResponse>(string serviceId, string name, object message, int timeout = 5000)
    {

        _channel.QueueDeclare($"{serviceId}.{name}.reply-to",
                   durable: true,
                   exclusive: false,
                   autoDelete: true,
               arguments: null);

        _channel.QueueBind($"{serviceId}.{name}.reply-to", "Optimum.EXC", $"{serviceId}.{name}.reply-to");
        _channel.BasicQos(0, 10, false);

        var _replayconsumer = new EventingBasicConsumer(_channel);
        
        _replayconsumer.Received += (sender, e) =>
        {
            var array = e.Body.ToArray();
            var value = Encoding.UTF8.GetString(array);
            respQueue.Add(value);
        };

        _channel.BasicConsume($"{serviceId}.{name}.reply-to", true, consumer: _replayconsumer);


        await BasicSend($"{serviceId}.{name}", message, $"{serviceId}.{name}.reply-to");

        respQueue.TryTake(out var data, timeout
            , CancellationToken.None);

        if (data == null) return default(TResponse);
        var res = JsonConvert.DeserializeObject<TResponse>(data);
        return res;
    }


    public async Task BasicSend(string route , object message , string replayRoute = null)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        prop.ReplyTo = replayRoute;
        _channel.BasicPublish("Optimum.EXC",
            routingKey: route,
            basicProperties: prop,
            body: body);
    }

}