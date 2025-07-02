namespace CaseR;

public interface IDomainEventPublisher
{
    Task Publish<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}