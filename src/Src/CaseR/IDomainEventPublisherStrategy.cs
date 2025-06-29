namespace CaseR;

public interface IDomainEventPublisherStrategy
{
    ValueTask Publish<TEvent>(IEnumerable<IDomainEventHandler<TEvent>> handlers,
        TEvent domainEvent,
        CancellationToken cancellationToken)
        where TEvent : IDomainEvent;
}