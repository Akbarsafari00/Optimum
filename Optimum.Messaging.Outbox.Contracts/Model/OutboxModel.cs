using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Messaging.Outbox.Contracts.Model
{
    public class OutboxModel
    {
        public OutboxModel(string serviceId, string type, string payload, string name)
        {
            ServiceId = serviceId;
            Type = type;
            Payload = payload;
            Name = name;
        }

        public string ServiceId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        
        
    }
}
