namespace Optimum.Cqrs.Contracts.Events;

public interface IEventDispatcher
{
    Task PublishAsync<T>(T command) where T : class, IEvent;
}