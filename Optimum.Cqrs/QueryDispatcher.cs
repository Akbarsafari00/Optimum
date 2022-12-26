using Microsoft.Extensions.DependencyInjection;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Queries;

namespace Optimum.Cqrs;

internal  class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceScopeFactory _serviceFactory;

    public QueryDispatcher(IServiceScopeFactory serviceFactory) => this._serviceFactory = serviceFactory;

    public async  Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope = this._serviceFactory.CreateScope();
        var type = typeof (IQueryHandler<,>).MakeGenericType(query.GetType(), typeof (TResult));
        var requiredService = scope.ServiceProvider.GetRequiredService(type);
        var method = type.GetMethod("HandleAsync");
        object obj;
        if ((object) method == null)
            obj = (object) null;
        else
            obj = method.Invoke(requiredService, new object[1]
            {
                (object) query
            });
        var result = await ((Task<TResult>) obj)!;
        return result;
    }

    public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
    {
        using var scope = this._serviceFactory.CreateScope();
        var result = await scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>().HandleAsync(query);
        return result;
    }
}