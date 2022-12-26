using Newtonsoft.Json;
using Optimum.Messaging.RabbitMQ.Options;
using Optimum.ServiceDiscovery.Contracts;
using Optimum.ServiceDiscovery.Models;
using System.Text.Json.Serialization;

namespace Optimum.ServiceDiscovery
{
    public class ServiceDiscovery : IServiceDiscovery
    {

        private readonly ServiceDiscoveryOptions serviceDiscoveryOptions;

        public ServiceDiscovery(ServiceDiscoveryOptions serviceDiscoveryOptions)
        {
            this.serviceDiscoveryOptions = serviceDiscoveryOptions;
        }

        public async Task<Service?> GetServiceAsync(string id)
        {
            try
            {
                var http = new HttpClient();
                var res = await http.GetAsync(serviceDiscoveryOptions.Uri + "/service/" + id);
                var content = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Service>(content);
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
