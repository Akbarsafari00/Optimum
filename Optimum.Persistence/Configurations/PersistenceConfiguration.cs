using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;
using Optimum.Persistence.Contracts;

namespace Optimum.Persistence.Configurations;

public class PersistenceConfiguration : IPersistenceConfiguration
{
    public IOptimumBuilder Builder { get; }

    public PersistenceConfiguration(IOptimumBuilder builder)
    {
        Builder = builder;
    }
}