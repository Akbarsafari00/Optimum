using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Optimum.Contracts;
using Optimum.Persistence.Contracts;
using Optimum.Persistence.MongoDB.Configurations;
using Optimum.Persistence.MongoDB.Contracts;
using Optimum.Persistence.MongoDB.Initializers;
using Optimum.Persistence.MongoDB.Options;
using Optimum.Persistence.MongoDB.Repositories;
using Optimum.Persistence.MongoDB.Seeders;

namespace Optimum.Persistence.MongoDB;

public static class Extensions
{

    public static List<Type> SeederTypes = new List<Type>();
    public static void UseMongo(this IPersistenceConfiguration persistenceConfiguration,Action<IMongoConfiguration> action )
    {
        var configuration = persistenceConfiguration.Builder.Services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();

        var mongoOptions = configuration.GetSection("Mongo").Get<MongoDbOptions>();

        if (mongoOptions == null)
        {
            throw new InvalidDataException("Please Insert 'Mongo' Section in 'appsettings.json' file.");
        }

        persistenceConfiguration.Builder.Services.AddSingleton(mongoOptions);
        persistenceConfiguration.Builder.Services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetService<MongoDbOptions>();
            var client = new MongoClient(options?.ConnectionString);
            persistenceConfiguration.Builder.Logger.Information(
                $"MongoDb Client Connected: {options.ConnectionString}");
            return client;
        });

        persistenceConfiguration.Builder.Services.AddTransient(sp =>
        {
            var options = sp.GetService<MongoDbOptions>();
            var client = sp.GetService<IMongoClient>();
            persistenceConfiguration.Builder.Logger.Information($"MongoDb Database Connected: {options.Database}");
            var database = client?.GetDatabase(options.Database);
            return database;
        });

        persistenceConfiguration.Builder.Services.AddTransient<IMongoDbInitializer, MongoDbInitializer>();

        

        persistenceConfiguration.Builder.AddInitializer<IMongoDbInitializer>();

        RegisterConventions();

        action?.Invoke(new MongoConfiguration(persistenceConfiguration.Builder));
    }

    public static void AddMongoRepository<TEntity, TIdentifiable>(this IMongoConfiguration mongoConfiguration,
        string collectionName)
        where TEntity : IIdentifiable<TIdentifiable>
    {
        mongoConfiguration.Builder.Services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
        {
            var database = sp.GetService<IMongoDatabase>();
            return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
        });
    }
    public static void AddMongoSeeder<TSeeder>(this IMongoConfiguration mongoConfiguration)
        where TSeeder : IMongoDbSeeder
    {
        mongoConfiguration.Builder.Services.AddSingleton(typeof(IMongoDbSeeder), typeof(TSeeder));
    }
    
    
    private static void RegisterConventions()
    {
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
        BsonSerializer.RegisterSerializer(typeof(decimal?),
            new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
        ConventionRegistry.Register("convey", new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String),
        }, _ => true);
    }
}