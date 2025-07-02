namespace CaseR;

public interface IDomainEventPublisherStrategy
{
    Task Publish<TEvent>(IEnumerable<IDomainEventHandler<TEvent>> handlers,
        TEvent domainEvent,
        CancellationToken cancellationToken)
        where TEvent : IDomainEvent;
}