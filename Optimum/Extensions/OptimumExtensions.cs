using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;
using Optimum.Options;
using ILogger = Serilog.ILogger;

namespace Optimum.Extensions;

public static class OptimumExtensions
{
  
    public static IOptimumBuilder AddOptimum(this IServiceCollection services )
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        var options = configuration.GetSection("Service").Get<ServiceOptions>();

        if (options==null)
        {
            throw new InvalidDataException("Plasee Insert 'Service' Section in 'appsettings.json' file.");
        }

        services.AddLogging();
        
        services.AddSingleton(options);

   

        var logger = services.BuildServiceProvider().GetRequiredService<ILogger>();
        
        var builder = new OptimumBuilder(services, options,logger);

        // action?.Invoke(builder);

        logger.Information($"Service Start Successful [id:{options.Id}] [name:{options.Name}] [version: {options.Version}]");
        
        return builder;
    }
    
    public static IApplicationBuilder UseOptimum(this IApplicationBuilder app )
    {
        using var scope = app.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IStartupInitializer>();
        Task.Run((Func<Task>) (() => initializer.InitializeAsync())).GetAwaiter().GetResult();
        return app;
    }
    
 
}