using CaseR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddCaseR(this IServiceCollection services, Action<CaseROptions>? configure = null)
    {
        CaseROptions options = new CaseROptions(null);
        configure?.Invoke(options);

        services.TryAddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.TryAddScoped<IDomainEventPublisherStrategy, ForeachAwaitPublisherStrategy>();

        services.AddScoped(typeof(IUseCase<>), typeof(UseCase<>));
        services.AddScoped(typeof(IUseCase<,,>), typeof(UseCase3<,,>));
        options.Register(services);
    }

    public static void AddCaseRDomainEventHandler<TEvent, THandler>(this IServiceCollection services)
        where TEvent : IDomainEvent
        where THandler : class, IDomainEventHandler<TEvent>
    {
       services.TryAddScoped<IDomainEventHandler<TEvent>, THandler>();
    }

    public static void AddKeyedCaseR(this IServiceCollection services, string serviceKey, Action<CaseROptions>? configure = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(serviceKey, nameof(serviceKey));

        CaseROptions options = new CaseROptions(serviceKey);
        configure?.Invoke(options);

        services.TryAddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.TryAddScoped<IDomainEventPublisherStrategy, ForeachAwaitPublisherStrategy>();

        services.AddKeyedScoped(typeof(IUseCase<>), serviceKey, typeof(KeyedUseCase<>));

        services.AddKeyedScoped(typeof(IUseCase<,,>), serviceKey, typeof(KeyedUseCase3<,,>));
        options.Register(services);
    }
}
