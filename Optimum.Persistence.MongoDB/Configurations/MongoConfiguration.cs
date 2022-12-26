using Optimum.Contracts;
using Optimum.Persistence.MongoDB.Contracts;

namespace Optimum.Persistence.MongoDB.Configurations;

public class MongoConfiguration : IMongoConfiguration
{
    public IOptimumBuilder Builder { get; }

    public MongoConfiguration(IOptimumBuilder builder)
    {
        Builder = builder;
    }
}