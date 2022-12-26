using System.Reflection;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Optimum.Hangfire.Contracts;
using Optimum.Messaging.Attributes;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.Outbox.Contracts;
using Optimum.Messaging.Outbox.Jobs;
using Optimum.Options;
using Optimum.Utilitis;

namespace Optimum.Messaging.Outbox.HostedServices
{
    public class OutboxHostedService : IHostedService
    {
        ServiceOptions _serviceOptions;
       
        private IHangfireService _hangfireService;

        public OutboxHostedService(ServiceOptions serviceOptions, IHangfireService hangfireService)
        {
            _serviceOptions = serviceOptions;
            _hangfireService = hangfireService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
           await _hangfireService.RecurringJobs<OutboxJob>("*/2 * * * * *", x=>x.RunAsync());
        }

      

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}