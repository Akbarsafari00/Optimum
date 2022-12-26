using Optimum.Cqrs.Contracts.Commands;
using Optimum.Cqrs.Contracts.Events;
using Optimum.Cqrs.Contracts.Queries;

namespace Optimum.Cqrs.Contracts;

public interface ICqrsDispatcher : IEventDispatcher, IQueryDispatcher, ICommandDispatcher
{
}