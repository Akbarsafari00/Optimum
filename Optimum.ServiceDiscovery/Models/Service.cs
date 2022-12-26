namespace Optimum.ServiceDiscovery.Models
{
    public class Service
    {
        public Service(string name, string id, string healthCheck, string address)
        {
            Name = name;
            Id = id;
            HealthCheck = healthCheck;
            Address = address;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string HealthCheck { get; set; }
        public string Address { get; set; }
    }
}
