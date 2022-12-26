

using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Optimum.Contracts;
using Optimum.Hangfire.Contracts;
using Optimum.Messaging.RabbitMQ.Options;
using System.Net.Http.Json;

namespace Optimum.Hangfire.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static IOptimumBuilder AddHangfire(this IOptimumBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var options = configuration.GetSection("Optimum").GetSection("Hangfire").Get<HangfireOptions>();

        if (options == null)
        {
            throw new InvalidDataException("Please Insert 'Optimum > Hangfire' Section in 'appsettings.json' file.");
        }
        builder.Services.AddSingleton(options);
        builder.Services.AddHangfire(configuration => configuration
     .UseSimpleAssemblyNameTypeSerializer()
     .UseRecommendedSerializerSettings()
     .UseInMemoryStorage());

        builder.Services.AddHangfireServer();


        builder.Services.AddTransient<IHangfireService, HangfireService>();

        return builder;
    }


}
