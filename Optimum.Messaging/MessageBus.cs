using Microsoft.Extensions.DependencyInjection;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.Outbox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Messaging
{
    public class MessageBus : IMessageBus
    {

        private readonly IMessagePublisher _messagePublisher;
        private readonly IOutboxStore _outboxStore  = null;

        public MessageBus(IMessagePublisher messagePublisher, IServiceProvider _serviceProvider)
        {
            _messagePublisher = messagePublisher;


            try
            {
                _outboxStore = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IOutboxStore>();
            }
            catch{}
        }

        public Task PublishAsync<T>(T @event)
        {
            if (_outboxStore == null)
            {
                return _messagePublisher.PublishAsync(@event);
            }
            else
            {
                return _outboxStore.StoreAsync("Publish", @event);
            }
        }

        public Task<TResponse?> RequestAsync<TMessage, TResponse>(string serviceId, TMessage message, int timeout = 5000)
        {
            return _messagePublisher.RequestAsync<TMessage, TResponse>(serviceId, message, timeout);
        }

        public Task SendAsync<T>(string serviceId, T message)
        {
            if (_outboxStore == null)
            {
                return _messagePublisher.SendAsync(serviceId, message);
            }
            else
            {
                return _outboxStore.StoreAsync("Send",message);
            }
        }
    }
}
