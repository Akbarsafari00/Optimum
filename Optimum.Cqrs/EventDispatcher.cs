using Microsoft.Extensions.DependencyInjection;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Events;

namespace Optimum.Cqrs;

internal  class EventDispatcher : IEventDispatcher
{
    private readonly IServiceScopeFactory _serviceFactory;

    public EventDispatcher(IServiceScopeFactory serviceFactory) => this._serviceFactory = serviceFactory;

    public async Task PublishAsync<T>(T command) where T : class, IEvent
    {
        using var scope = this._serviceFactory.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IEventHandler<T>>().HandleAsync(command);
    }
}