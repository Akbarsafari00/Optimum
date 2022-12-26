using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optimum.Contracts;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Optimum.Extensions;

public static class  LoggerExtensions
{
    internal static LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch();
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder)
        => hostBuilder
            .ConfigureServices(services => services.AddSingleton<ILoggingService, LoggingService>())
            .UseSerilog((ctx, lc) =>
            {
                lc.WriteTo.Console();
            });
    
    internal static LogEventLevel GetLogEventLevel(string level)
        => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
            ? logLevel
            : LogEventLevel.Information;
}