using Optimum.Messaging;
using Optimum.Messaging.Contracts;
using Optimum.Messaging;
using Optimum.Test.Api.Documents;

namespace Optimum.Test.Api.Consumers;

public class WeatherForecastConsumer2 : IMessageConsumer<WeatherForecast2>
{
    public async Task HandleAsync(MessageContext<WeatherForecast2> context)
    {
        Console.WriteLine(context.CorrelationId);
    }
}