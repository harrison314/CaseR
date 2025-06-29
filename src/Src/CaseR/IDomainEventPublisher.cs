namespace CaseR;

public interface IDomainEventPublisher
{
    ValueTask Publish<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}