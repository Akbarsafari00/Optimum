using Microsoft.Extensions.DependencyInjection;

using Optimum.Options;
using Serilog;

namespace Optimum.Contracts;

public interface IOptimumBuilder
{
    public IServiceCollection Services { get;  }
    public ServiceOptions Options { get; }
    public ILogger Logger { get; }

    void AddBuildAction(Action<IServiceProvider> execute);
    
    void AddInitializer(IInitializer? initializer);

    void AddInitializer<TInitializer>() where TInitializer : IInitializer;

    public IServiceCollection Build();
}