using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;

namespace Optimum.Persistence.Contracts;

public interface IPersistenceConfiguration
{
    IOptimumBuilder Builder { get; }


}