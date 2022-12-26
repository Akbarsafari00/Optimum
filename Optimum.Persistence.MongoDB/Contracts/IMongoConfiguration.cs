using Optimum.Contracts;

namespace Optimum.Persistence.MongoDB.Contracts;

public interface IMongoConfiguration
{
    IOptimumBuilder Builder { get; }
}