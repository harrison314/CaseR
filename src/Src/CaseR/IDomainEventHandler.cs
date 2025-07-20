namespace CaseR;

/// <summary>
/// Defines a handler for processing domain events of a specific type.
/// </summary>
/// <typeparam name="TEvent">The type of the domain event to handle. Must implement <see cref="IDomainEvent"/>.</typeparam>
public interface IDomainEventHandler<TEvent>
    where TEvent : IDomainEvent
{
    /// <summary>
    /// Handles the specified domain event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event to process. Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation should be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the domain event has been processed.</returns>
    Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}