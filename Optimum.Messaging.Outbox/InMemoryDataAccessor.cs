using Newtonsoft.Json;
using Optimum.Messaging.Outbox.Contracts;
using Optimum.Messaging.Outbox.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Messaging.Outbox
{
    public class InMemoryDataAccessor : IDataAccessor
    {

        private List<OutboxModel> items = new List<OutboxModel>();

        public Task<IEnumerable<OutboxModel>> GetAll()
        {
            return Task.FromResult(items.AsEnumerable());
        }

        public Task<OutboxModel> RemoveAsync(OutboxModel data)
        {
            items.Remove(data);
            return Task.FromResult(data);
        }

        public async Task<OutboxModel> SaveAsync(OutboxModel data)
        {
            items.Add(data);
            return data;
        }
    }
}
