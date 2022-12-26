using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Optimum.Persistence.MongoDB.Contracts;
using Optimum.Persistence.MongoDB.Options;

namespace Optimum.Persistence.MongoDB.Initializers
{
    internal sealed class MongoDbInitializer : IMongoDbInitializer
    {
        private static int _initialized;
        private readonly MongoDbOptions _dbOptions;
        private readonly IMongoDatabase _database;
        private readonly IServiceProvider _serviceProvider;

        public MongoDbInitializer(IMongoDatabase database, MongoDbOptions dbOptions, IServiceProvider serviceProvider)
        {
            _database = database;
            _serviceProvider = serviceProvider;
            _dbOptions = dbOptions;
        }

        public Task InitializeAsync()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 1)
            {
                return Task.CompletedTask;
            }

            try
            {
                if (_dbOptions.Seed == false)
                    return Task.CompletedTask;;
                
                var seeder = _serviceProvider.GetRequiredService<IMongoDbSeeder>();
                return seeder.SeedAsync(_database);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on Find IMongoDbSeeder");
            }

            return Task.CompletedTask;
        }
    }
}