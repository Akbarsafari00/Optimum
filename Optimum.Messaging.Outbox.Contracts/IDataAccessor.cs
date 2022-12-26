using Optimum.Messaging.Outbox.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Messaging.Outbox.Contracts
{
    public interface IDataAccessor
    {
        Task<OutboxModel> SaveAsync(OutboxModel data);
        Task<OutboxModel> RemoveAsync(OutboxModel data);
        Task<IEnumerable<OutboxModel>> GetAll();
    }
}
