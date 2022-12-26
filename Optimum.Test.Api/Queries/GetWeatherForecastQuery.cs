using MongoDB.Bson;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Queries;
using Optimum.Persistence.MongoDB;
using Optimum.Test.Api.Documents;

namespace Optimum.Test.Api.Queries;

public class GetWeatherForecastQuery : IQuery<IEnumerable<WeatherForecast>>
{
}

public class GetWeatherForecastQueryHandler : IQueryHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecast>>
{
    private readonly IMongoRepository<WeatherForecast, Guid> _weatherForecastRepository;

    public GetWeatherForecastQueryHandler(IMongoRepository<WeatherForecast, Guid> weatherForecastRepository)
    {
        _weatherForecastRepository = weatherForecastRepository;
    }

    public async Task<IEnumerable<WeatherForecast>> HandleAsync(GetWeatherForecastQuery query)
    {
        var res = await _weatherForecastRepository.FindAsync(x => true);
        return res.ToList();
    }
}