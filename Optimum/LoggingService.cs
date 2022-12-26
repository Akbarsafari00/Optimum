using Optimum.Contracts;
using Optimum.Extensions;

namespace Optimum;

public class LoggingService : ILoggingService
{
    public void SetLoggingLevel(string logEventLevel)
    {
        LoggerExtensions.LoggingLevelSwitch.MinimumLevel = LoggerExtensions.GetLogEventLevel(logEventLevel);
    }
}