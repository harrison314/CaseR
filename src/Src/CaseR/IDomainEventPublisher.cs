namespace CaseR;

/// <summary>
/// Defines a mechanism for publishing domain events to interested subscribers.
/// </summary>
public interface IDomainEventPublisher
{
    /// <summary>
    /// Publish a domain event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent">Domain event type.</typeparam>
    /// <param name="domainEvent">Domain event instance.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation should be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the all domain events has been processed.</returns>
    Task Publish<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}