using Optimum.Messaging;
using Optimum.Messaging.Contracts;
using Optimum.Messaging;
using Optimum.Test.Api.Documents;

namespace Optimum.Test.Api.Consumers;

public class WeatherForecastConsumer : IMessageConsumer<WeatherForecast>
{
    public async Task HandleAsync(MessageContext<WeatherForecast> context)
    {
        Console.WriteLine(context.CorrelationId);

        await context.RespondeAsync(context.Message);
    }
}