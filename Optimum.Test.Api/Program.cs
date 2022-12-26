using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Optimum.Authentication.Jwt;
using Optimum.Cqrs.Extensions;
using Optimum.Extensions;
using Optimum.Hangfire.Extensions;
using Optimum.Http.Extensions;
using Optimum.Messaging;
using Optimum.Messaging.RabbitMQ;
using Optimum.Persistence.Extensions;
using Optimum.Persistence.MongoDB;
using Optimum.Persistence.MongoDB.Contracts;
using Optimum.Swagger;
using Optimum.Test.Api;
using Optimum.Test.Api.Consumers;
using Optimum.Test.Api.Documents;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLogging();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddOptimum()
    .AddCqrs()
    .AddPersistence(x =>
    {
        x.UseMongo(c =>
        {
            c.AddMongoRepository<WeatherForecast, Guid>(nameof(WeatherForecast));
            c.AddMongoSeeder<Seed>();
        });
    })
    .AddMessageBroker(c =>
    {
        c.UseRabbitMq();
        c.UseInMemoryOutbox();
    })
    .AddServiceDiscovery()
    .AddHangfire()
    .AddHttpService()
    .AddSwagger()
    .AddJwtAuthentication()
    .Build();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOptimumSwagger();
}

app.UseHttpsRedirection();

app.UseOptimumJwtAuthentication();

app.MapControllers();

app.UseOptimum();
app.UseHangfire();

app.Run();

namespace Optimum.Test.Api
{
    class Seed : IMongoDbSeeder
    {
        public async Task SeedAsync(IMongoDatabase database)
        {
            var c = database.GetCollection<WeatherForecast>("WeatherForecast");
            await c.InsertManyAsync(new[]
            {
                new WeatherForecast()
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Summary = "sad",
                    TemperatureC = 10
                }
            });
        }
    }
}