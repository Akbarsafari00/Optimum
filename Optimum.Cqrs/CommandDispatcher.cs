using Microsoft.Extensions.DependencyInjection;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Commands;

namespace Optimum.Cqrs;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceScopeFactory _serviceFactory;

    public CommandDispatcher(IServiceScopeFactory serviceFactory) => this._serviceFactory = serviceFactory;

    public async Task SendAsync<T>(T command) where T : class, ICommand
    {
        using var scope = this._serviceFactory.CreateScope();
        await scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>().HandleAsync(command);
    }
}