using Optimum.Messaging.Outbox.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Messaging.Outbox.Contracts
{
    public interface IOutboxStore
    {
        Task<OutboxModel> StoreAsync<T>(string type , T store);
        Task<OutboxModel> RemoveAsync(OutboxModel model);
        Task<IEnumerable<OutboxModel>> GetAllAsync();
    }
}
