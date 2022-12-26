

using Microsoft.Extensions.Configuration;
using Optimum.Contracts;
using Optimum.Persistence.Configurations;
using Optimum.Persistence.Contracts;

namespace Optimum.Persistence.Extensions;

public static class OptimumPersistenceExtensions
{
    public static IOptimumBuilder AddPersistence(this IOptimumBuilder builder,Action<IPersistenceConfiguration> configuration)
    {
        configuration(new PersistenceConfiguration(builder));
        return builder;
    }

    
}
