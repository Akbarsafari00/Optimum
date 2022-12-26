using Microsoft.AspNetCore.Mvc;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Commands;
using Optimum.Cqrs.Contracts.Queries;
using Optimum.Messaging.Contracts;
using Optimum.Test.Api.Commands;
using Optimum.Test.Api.Documents;
using Optimum.Test.Api.Queries;

namespace Optimum.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
   

    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessagePublisher _messageBus;


    public WeatherForecastController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IMessagePublisher messageBus)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _messageBus = messageBus;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<WeatherForecast?> Get()
    {
      var res =  await _messageBus.RequestAsync<WeatherForecast,WeatherForecast>("test-api",new WeatherForecast() { Id = Guid.NewGuid(),TemperatureC = 12});
        return res;
    }
    
    [HttpPost(Name = "CreateWeatherForecast")]
    public async Task Create([FromBody] CreateWeatherForecastCommand command)
    {
        await _messageBus.SendAsync("test-api", command);
    }
}