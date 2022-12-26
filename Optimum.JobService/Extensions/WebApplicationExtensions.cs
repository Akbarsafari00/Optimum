

using Hangfire;
using Microsoft.AspNetCore.Builder;
using System.Net.Http.Json;

namespace Optimum.Hangfire.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseHangfire(this WebApplication app)
    {

        app.UseHangfireDashboard();

        return app;
    }

    
}
