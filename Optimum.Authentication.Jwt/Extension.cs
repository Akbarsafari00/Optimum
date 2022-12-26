


using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Optimum.Authentication.Jwt.Contracts;
using Optimum.Authentication.Jwt.Options;
using Optimum.Contracts;

namespace Optimum.Authentication.Jwt;

public static class Extension
{
    public static IOptimumBuilder AddJwtAuthentication(this IOptimumBuilder builder)
    {
        var configuration = builder.Services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();

        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

        if (jwtOptions == null)
        {
            throw new InvalidDataException("Please Insert 'Jwt' Section in 'appsettings.json' file.");
        }

        builder.Services.AddSingleton(jwtOptions);
        
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(jwtOptions.Key);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddTransient<IJwtService, JwtService>();
        
        return builder;
    }

    public static IApplicationBuilder UseOptimumJwtAuthentication(this IApplicationBuilder app )
    {
        app.UseAuthentication(); // This need to be added	
        app.UseAuthorization();
        return app;
    }

}
