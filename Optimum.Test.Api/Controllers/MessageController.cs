using Microsoft.AspNetCore.Mvc;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Commands;
using Optimum.Cqrs.Contracts.Queries;
using Optimum.Messaging.Contracts;
using Optimum.Messaging.Outbox.Contracts;
using Optimum.Messaging.Outbox.Contracts.Model;
using Optimum.Options;
using Optimum.ServiceDiscovery.Contracts;
using Optimum.ServiceDiscovery.Models;
using Optimum.Test.Api.Commands;
using Optimum.Test.Api.Documents;
using Optimum.Test.Api.Queries;

namespace Optimum.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{


    private readonly IMessageBus _messageBus;
    private readonly IOutboxStore _outboxStore;
    private readonly ServiceOptions _serviceOptions;

    public MessageController(IMessageBus messageBus, ServiceOptions serviceOptions, IOutboxStore outboxStore)
    {
        _messageBus = messageBus;
        _serviceOptions = serviceOptions;
        _outboxStore = outboxStore;
    }

    [HttpPost("Publish")]
    public async Task Publish()
    {
        await _messageBus.PublishAsync(new WeatherForecast { Id = Guid.NewGuid(),Date = DateTime.Now,Summary = "Tet"});
    }


    [HttpPost("Request")]
    public async Task<WeatherForecast> Request()
    {
        var res =  await _messageBus.RequestAsync<WeatherForecast, WeatherForecast>(_serviceOptions.Id, new WeatherForecast { Id = Guid.NewGuid(), Date = DateTime.Now, Summary = "Tet" });
        return res;
    }


    [HttpGet("Outbox/All")]
    public async Task<IEnumerable<OutboxModel>> OutboxGetAll()
    {
        return await _outboxStore.GetAllAsync();
    }

}