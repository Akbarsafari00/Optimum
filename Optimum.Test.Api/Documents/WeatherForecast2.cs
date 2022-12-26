using MongoDB.Bson;
using Optimum.Contracts;

namespace Optimum.Test.Api.Documents;

public class WeatherForecast2 : IIdentifiable<Guid>
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
    public Guid Id { get; set; }
}