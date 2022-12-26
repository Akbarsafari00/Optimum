

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;
using Optimum.Messaging.RabbitMQ.Options;
using Optimum.Options;
using Optimum.ServiceDiscovery;
using Optimum.ServiceDiscovery.Contracts;
using Optimum.ServiceDiscovery.Models;
using System.Net.Http.Json;

namespace Optimum.ServiceDiscovery.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseServiceDiscovery(this WebApplication app)
    {
        var services = new List<Service>();

        app.MapGet("/services", () =>
        {
            return services;
        });

        app.MapGet("/service/{id}", (string id) =>
        {
            return services.FirstOrDefault(x => x.Id == id);
        });

        app.MapPost("/service/register", ([FromBody] Service service) =>
        {

            if (!services.Any(x => x.Id == service.Id))
            {
                services.Add(service);
            }

            return service;
        }); 
        return app;
    }

    
}
