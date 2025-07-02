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

    public async Task Publish<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        IEnumerable<IDomainEventHandler<TEvent>>? handlers = this.serviceProvider.GetService<IEnumerable<IDomainEventHandler<TEvent>>>();
        if (handlers != null)
        {
            await this.publisherStrategy.Publish(handlers, domainEvent, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
