using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Optimum.Contracts;
using Optimum.Swagger.Options;

namespace Optimum.Swagger;

public static class Extensions
{
    public static IOptimumBuilder AddSwagger(this IOptimumBuilder builder)
    {
        var configuration = builder.Services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();

        var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerOptions>();

        if (swaggerOptions == null)
        {
            throw new InvalidDataException("Please Insert 'Swagger' Section in 'appsettings.json' file.");
        }

        builder.Services.AddSingleton(swaggerOptions);
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerOptions.Name, new OpenApiInfo { Title = swaggerOptions.Title, Version = swaggerOptions.Version});

            if (swaggerOptions.BearerAuth)
            {
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
    
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            }


        });
        
        return builder;
    }

    public static IApplicationBuilder UseOptimumSwagger(this IApplicationBuilder app )
    {
        var swaggerOptions = app.ApplicationServices.GetRequiredService<SwaggerOptions>();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{swaggerOptions.Name.ToLower().Trim()}/swagger.json",swaggerOptions.Name.ToLower().Trim());
        });
        return app;
    }

}
