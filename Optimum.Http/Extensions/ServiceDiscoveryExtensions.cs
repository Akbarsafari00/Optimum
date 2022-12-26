

using Microsoft.Extensions.DependencyInjection;
using Optimum.Contracts;
using Optimum.Http.Contracts;

namespace Optimum.Http.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static IOptimumBuilder AddHttpService(this IOptimumBuilder builder)
    {

        builder.Services.AddTransient<IHttpService, HttpService>();

        return builder;
    }

    
}
