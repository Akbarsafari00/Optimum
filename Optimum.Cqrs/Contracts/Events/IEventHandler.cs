namespace Optimum.Cqrs.Contracts.Events;

public interface IEventHandler<in TEvent> where TEvent:class,IEvent
{
    Task HandleAsync(TEvent @event);
}