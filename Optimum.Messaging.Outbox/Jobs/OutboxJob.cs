using Newtonsoft.Json;
using Optimum.Hangfire.Contracts;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.Outbox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Messaging.Outbox.Jobs
{
    public class OutboxJob 
    {
        private IOutboxStore _outboxStore;
        private IMessagePublisher _messagePublisher;

        public OutboxJob(IOutboxStore outboxStore, IMessagePublisher messagePublisher)
        {
            _outboxStore = outboxStore;
            _messagePublisher = messagePublisher;
        }

        public async Task RunAsync()
        {
            var data = await _outboxStore.GetAllAsync();
            if (data == null) return;
            foreach (var item in data.ToList())
            {
                if (item.Type == "Send")
                {
                    var obj = JsonConvert.DeserializeObject(item.Payload);
                    await _messagePublisher.SendAsync(item.ServiceId, item.Name, obj);
                    await _outboxStore.RemoveAsync(item);
                }
                else if (item.Type == "Publish")
                {
                    var obj = JsonConvert.DeserializeObject(item.Payload);
                    await _messagePublisher.PublishAsync(item.Name, obj);
                    await _outboxStore.RemoveAsync(item);
                }
            }
        }
    }
}
