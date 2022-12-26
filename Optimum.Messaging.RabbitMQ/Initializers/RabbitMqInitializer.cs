using Optimum.Messaging.RabbitMQ.Contracts;

namespace Optimum.Messaging.RabbitMQ.Initializers
{
    internal sealed class RabbitMqInitializer : IRabbitMqInitializer
    {
       
        public Task InitializeAsync()
        {
            
            return Task.CompletedTask;
        }
    }
}