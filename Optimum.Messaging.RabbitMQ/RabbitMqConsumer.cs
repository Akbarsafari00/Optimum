using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Optimum.Messaging.RabbitMQ.Contracts;
using Optimum.Messaging.RabbitMQ.Options;
using Optimum.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Optimum.Messaging.RabbitMQ
{
    public class RabbitMqConsumer : IRabbitMqConsumer
    {
        RabbitMqOptions _rabbitMqOptions;
        ServiceOptions _serviceOptions;
        IServiceProvider _serviceProvider;
        IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumer(RabbitMqOptions rabbitMqOptions, ServiceOptions serviceOptions,
            IServiceProvider serviceProvider, IConnection connection, IModel channel)
        {
            _rabbitMqOptions = rabbitMqOptions;
            _serviceOptions = serviceOptions;
            _serviceProvider = serviceProvider;
            _connection = connection;
            _channel = channel;
        }

        public void Consume<T>(string serviceId, Action<T> action)
        {
            try
            {
                _channel.QueueDeclare(
                    typeof(T).Name,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(_channel);

                _channel.QueueBind(typeof(T).Name, serviceId, "rk:" + _serviceOptions.Id);
                _channel.BasicQos(0, 10, false);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var res = JsonConvert.DeserializeObject<T>(message);
                    if (res != null) action.Invoke(res);
                    _channel.BasicAck(e.DeliveryTag, false);
                };

                _channel.BasicConsume(typeof(T).Name, true, consumer);
                Console.WriteLine($"Consumer {typeof(T)} Started ");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Consumer {typeof(T)} Failed ");
                Console.WriteLine(e.Message);
            }
        }

        public void ConsumeWithResult<TMessage>(string serviceId, Action<RMQMessageContext<TMessage>> action)
        {
            try
            {
                _channel.QueueDeclare($"{serviceId}.{typeof(TMessage).Name}",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                arguments: null);
               


                _channel.QueueBind($"{serviceId}.{typeof(TMessage).Name}", "Optimum.EXC", $"{serviceId}.{typeof(TMessage).Name}");
                _channel.BasicQos(0, 10, false);

                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var res = JsonConvert.DeserializeObject<TMessage>(message);
                    if (res == null) return;

                    var ctx = new RMQMessageContext<TMessage>(_channel, res, e.BasicProperties);
                    action.Invoke(ctx);

                    _channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);


                };

                _channel.BasicConsume($"{serviceId}.{typeof(TMessage).Name}", false, consumer);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Consumer {typeof(TMessage)} Failed ");
                Console.WriteLine(e.Message);
            }
        }
        public async Task<T> ConsumeReply<T>(string serviceId)
        {
            var tcs = new TaskCompletionSource<T>();
            try
            {
                _channel.QueueDeclare("Reply:" + typeof(T).Name,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var consumer = new EventingBasicConsumer(_channel);

                _channel.QueueBind("Reply:" + typeof(T).Name, serviceId, "rp-rk:" + _serviceOptions.Id);
                _channel.BasicQos(0, 10, false);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var res = JsonConvert.DeserializeObject<T>(message);
                    if (res != null)
                        tcs.TrySetResult(res);

                    _channel.QueueDelete("Reply:" + typeof(T).Name);
                    _channel.BasicAck(e.DeliveryTag, false);
                };


                _channel.BasicConsume("Reply:" + typeof(T).Name, true, consumer);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Consumer {typeof(T)} Failed ");
                Console.WriteLine(e.Message);
            }

            return await tcs.Task;
        }
    }
}