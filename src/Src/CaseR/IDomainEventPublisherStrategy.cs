namespace CaseR;

/// <summary>
/// Defines a strategy for publishing domain events to their respective handlers.
/// </summary>
public interface IDomainEventPublisherStrategy
{
    /// <summary>
    /// Publishes a domain event to the specified handlers asynchronously.
    /// </summary>
    /// <typeparam name="TEvent">The type of the domain event. Must implement <see cref="IDomainEvent"/>.</typeparam>
    /// <param name="handlers">The collection of handlers that will process the domain event.</param>
    /// <param name="domainEvent">The domain event instance to publish.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task Publish<TEvent>(IEnumerable<IDomainEventHandler<TEvent>> handlers,
        TEvent domainEvent,
        CancellationToken cancellationToken)
        where TEvent : IDomainEvent;
}