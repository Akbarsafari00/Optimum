using Microsoft.Extensions.DependencyInjection;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Commands;
using Optimum.Cqrs.Contracts.Events;
using Optimum.Cqrs.Contracts.Queries;

namespace Optimum.Cqrs;

internal sealed class CqrsDispatcher : ICqrsDispatcher
{

    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IEventDispatcher _eventDispatcher;

    public CqrsDispatcher(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _eventDispatcher = eventDispatcher;
    }

    public  Task SendAsync<T>(T command) where T : class, ICommand 
        => _commandDispatcher.SendAsync(command);
   

    public Task PublishAsync<T>(T command) where T : class, IEvent
        => _eventDispatcher.PublishAsync(command);

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        => _queryDispatcher.QueryAsync(query);

    public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
        => _queryDispatcher.QueryAsync<TQuery,TResult>(query);
}