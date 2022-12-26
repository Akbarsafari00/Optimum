using Newtonsoft.Json;
using Optimum.Messaging.Outbox.Contracts;
using Optimum.Messaging.Outbox.Contracts.Model;
using Optimum.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Messaging.Outbox
{
    public class OutboxStore : IOutboxStore
    {
        private readonly IDataAccessor _dataAccessor;
        private readonly ServiceOptions _serviceOptions;

        public OutboxStore(IDataAccessor dataAccessor, ServiceOptions serviceOptions)
        {
            _dataAccessor = dataAccessor;
            _serviceOptions = serviceOptions;
        }

        public Task<IEnumerable<OutboxModel>> GetAllAsync()
        {
            return _dataAccessor.GetAll();
        }

        public Task<OutboxModel> RemoveAsync(OutboxModel model)
        {
            return _dataAccessor.RemoveAsync(model);
        }

        public Task<OutboxModel> StoreAsync<T>(string type ,T store)
        {
            return _dataAccessor.SaveAsync(new Contracts.Model.OutboxModel(
                _serviceOptions.Id, 
                type,
                JsonConvert.SerializeObject(store),
                typeof(T).Name
                ));
        }
    }
}
