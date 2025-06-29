namespace CaseR;

public sealed class ForeachAwaitPublisherStrategy : IDomainEventPublisherStrategy
{
    public ForeachAwaitPublisherStrategy()
    {

    }

    public async ValueTask Publish<TEvent>(IEnumerable<IDomainEventHandler<TEvent>> handlers,
        TEvent domainEvent,
        CancellationToken cancellationToken)
        where TEvent : IDomainEvent
    {
        if (handlers is IDomainEventHandler<TEvent>[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                ValueTask valueTask = array[i].Handle(domainEvent, cancellationToken);
                if (!valueTask.IsCompletedSuccessfully)
                {
                    await valueTask.ConfigureAwait(false);
                }
            }
        }
        else if (handlers is List<IDomainEventHandler<TEvent>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ValueTask valueTask = list[i].Handle(domainEvent, cancellationToken);
                if (!valueTask.IsCompletedSuccessfully)
                {
                    await valueTask.ConfigureAwait(false);
                }
            }
        }
        else
        {
            foreach (IDomainEventHandler<TEvent> handler in handlers)
            {
                ValueTask valueTask = handler.Handle(domainEvent, cancellationToken);
                if (!valueTask.IsCompletedSuccessfully)
                {
                    await valueTask.ConfigureAwait(false);
                }
            }
        }
    }
}