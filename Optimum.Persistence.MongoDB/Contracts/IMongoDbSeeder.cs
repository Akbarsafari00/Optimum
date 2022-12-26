using MongoDB.Driver;

namespace Optimum.Persistence.MongoDB.Contracts;

public interface IMongoDbSeeder
{
    Task SeedAsync(IMongoDatabase database);
}