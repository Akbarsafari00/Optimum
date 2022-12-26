namespace Optimum.Cqrs.Contracts.Events;

public interface IEvent
{
    public Guid  CorrelationId { get; set; }
}   