using Microsoft.Extensions.DependencyInjection;

namespace CaseR;

internal class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IServiceProvider serviceProvider;
    private readonly IDomainEventPublisherStrategy publisherStrategy;

    public DomainEventPublisher(
        IServiceProvider serviceProvider,
        IDomainEventPublisherStrategy publisherStrategy)
    {
        this.serviceProvider = serviceProvider;
        this.publisherStrategy = publisherStrategy;
    }

    public async ValueTask Publish<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        IEnumerable<IDomainEventHandler<TEvent>>? handlers = this.serviceProvider.GetService<IEnumerable<IDomainEventHandler<TEvent>>>();
        if (handlers != null)
        {
            ValueTask valueTask = this.publisherStrategy.Publish(handlers, domainEvent, cancellationToken);
            if (!valueTask.IsCompletedSuccessfully)
            {
                await valueTask.ConfigureAwait(false);
            }
        }
    }
}
