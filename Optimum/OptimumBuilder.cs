using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;
using Optimum.Options;
using Serilog;

namespace Optimum;

public class OptimumBuilder : IOptimumBuilder
{
    private readonly List<Action<IServiceProvider>> _buildActions;
    public OptimumBuilder(IServiceCollection services, ServiceOptions options, ILogger logger)
    {
        Services = services;
        Options = options;
        Logger = logger;
        _buildActions = new List<Action<IServiceProvider>>();
        Services.AddSingleton<IStartupInitializer>(new OptimumInitializer());
    }

    public IServiceCollection Services { get; }
    public ServiceOptions Options { get; }
    public ILogger Logger { get; }
    
    public void AddBuildAction(Action<IServiceProvider> execute)
    {
        _buildActions.Add(execute);
    }

    public void AddInitializer(IInitializer? initializer)
    {
        AddBuildAction(sp => sp.GetService<IStartupInitializer>()?.AddInitializer(initializer));;  
    } 

    public void AddInitializer<TInitializer>() where TInitializer : IInitializer => this.AddBuildAction(sp =>
    {
       
        
        var service = sp.GetService<TInitializer>();
        sp.GetService<IStartupInitializer>()?.AddInitializer(service);
    });

    public IServiceCollection Build()
    {
       
        var serviceProvider = Services.BuildServiceProvider();
        _buildActions.ForEach(a => a(serviceProvider));
        return Services;
    }
}