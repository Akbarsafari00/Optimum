using Optimum.ServiceDiscovery.Models;

namespace Optimum.ServiceDiscovery.Contracts
{
    public interface IServiceDiscovery
    {
        Task<Service> GetServiceAsync(string id);
    }

}
