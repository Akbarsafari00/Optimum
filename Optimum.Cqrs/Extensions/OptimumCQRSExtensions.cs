using Microsoft.Extensions.DependencyInjection;
using Optimum.Attributes;
using Optimum.Contracts;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Commands;
using Optimum.Cqrs.Contracts.Events;
using Optimum.Cqrs.Contracts.Queries;

namespace Optimum.Cqrs.Extensions;

public static class OptimumCqrsExtensions
{
    public static IOptimumBuilder AddCqrs(this IOptimumBuilder builder)
    {
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)).WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces().WithTransientLifetime());
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)).WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces().WithTransientLifetime());
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)).WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces().WithTransientLifetime());


        builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
        builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        builder.Services.AddSingleton<ICqrsDispatcher, CqrsDispatcher>();


        return builder;
    }
}