namespace CaseR;

public interface IDomainEventHandler<TEvent>
    where TEvent : IDomainEvent
{
    ValueTask Handle(TEvent domainEvent, CancellationToken cancellationToken);
}