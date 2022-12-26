

using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;
using Optimum.Messaging.RabbitMQ.Options;
using Optimum.Options;
using Optimum.ServiceDiscovery;
using Optimum.ServiceDiscovery.Contracts;
using Optimum.ServiceDiscovery.Models;
using System.Net.Http.Json;

namespace Optimum.Persistence.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static IOptimumBuilder AddServiceDiscovery(this IOptimumBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var appService = serviceProvider.GetRequiredService<ServiceOptions>();
        var options = configuration.GetSection("ServiceDiscovery").Get<ServiceDiscoveryOptions>();

        if (options == null)
        {
            throw new InvalidDataException("Please Insert 'ServiceDiscovery' Section in 'appsettings.json' file.");
        }

        builder.Services.AddSingleton(options);


        var http = new HttpClient();

        var content = JsonContent.Create(new Service(appService.Name,appService.Id,options.HealthCheck, options.Address));

        try
        {
            var res = http.PostAsync(options.Uri + "/service/register", content).GetAwaiter().GetResult();
            if (res.IsSuccessStatusCode)
            {
                builder.Logger.Information($"register service '{appService.Id}' on service discovery successfull :)");
            }
            else
            {
                builder.Logger.Error($"cannot register service '{appService.Id}' on service discovery :(");
            }
        }
        catch (Exception e)
        {
            var error = e.InnerException?.Message ?? e.Message;
            builder.Logger.Error($"cannot register service '{appService.Id}' on service discovery . {error}");
        }


        builder.Services.AddTransient<IServiceDiscovery, ServiceDiscovery.ServiceDiscovery>();

        return builder;
    }

    
}
