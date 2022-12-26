using Optimum.Cqrs.Contracts.Commands;
using Optimum.Persistence.MongoDB;
using Optimum.Test.Api.Documents;

namespace Optimum.Test.Api.Commands;

public class CreateWeatherForecastCommand : ICommand
{
    public string Summary { get; set; }
    public int TemperatureC { get; set; }
}

public class CreateWeatherForecastCommandHandler : ICommandHandler<CreateWeatherForecastCommand>
{
    private readonly IMongoRepository<WeatherForecast, Guid> _weatherForecastRepository;

    public CreateWeatherForecastCommandHandler(IMongoRepository<WeatherForecast, Guid> weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async Task HandleAsync(CreateWeatherForecastCommand command)
    {
        await _weatherForecastRepository.AddAsync(new WeatherForecast()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
            Summary = command.Summary,
            TemperatureC = command.TemperatureC
        });
    }
}