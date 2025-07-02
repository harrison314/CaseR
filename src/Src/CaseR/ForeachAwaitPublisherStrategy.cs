namespace CaseR;

public sealed class ForeachAwaitPublisherStrategy : IDomainEventPublisherStrategy
{
    public ForeachAwaitPublisherStrategy()
    {

    }

    public async Task Publish<TEvent>(IEnumerable<IDomainEventHandler<TEvent>> handlers,
        TEvent domainEvent,
        CancellationToken cancellationToken)
        where TEvent : IDomainEvent
    {
        if (handlers is IDomainEventHandler<TEvent>[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                await array[i].Handle(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }
        else if (handlers is List<IDomainEventHandler<TEvent>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                await list[i].Handle(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            foreach (IDomainEventHandler<TEvent> handler in handlers)
            {
                await handler.Handle(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}