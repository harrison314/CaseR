using CaseR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for registering CaseR services and domain event handlers in the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add CaseR services with pipeline to the service collection.
    /// </summary>
    /// <param name="services">IoC services.</param>
    /// <param name="configure">Configuration action.</param>
    public static void AddCaseR(this IServiceCollection services, Action<CaseROptions>? configure = null)
    {
        CaseROptions options = new CaseROptions(null);
        configure?.Invoke(options);

        services.TryAddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.TryAddScoped<IDomainEventPublisherStrategy, ForeachAwaitPublisherStrategy>();

        services.AddScoped(typeof(IUseCase<>), typeof(UseCase<>));
        services.AddTransient(typeof(IAutoScopedUseCase<>), typeof(AutoScopedUseCase<>));
        options.Register(services);
    }

    /// <summary>
    /// Add a CaseR domain event handler to the service collection.
    /// </summary>
    /// <typeparam name="TEvent">The domain event type.</typeparam>
    /// <typeparam name="THandler">The domain event handler type</typeparam>
    /// <param name="services">IoC services.</param>
    public static void AddCaseRDomainEventHandler<TEvent, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(this IServiceCollection services)
        where TEvent : IDomainEvent
        where THandler : class, IDomainEventHandler<TEvent>
    {
        services.TryAddScoped<IDomainEventHandler<TEvent>, THandler>();
    }

    /// <summary>
    /// Add keyed CaseR services with pipeline to the service collection.
    /// </summary>
    /// <param name="services">IoC services.</param>
    /// <param name="serviceKey">The <see cref="ServiceDescriptor.ServiceKey" /> of the registered services and pipeline.</param>
    /// <param name="configure">Configuration action.</param>
    public static void AddKeyedCaseR(this IServiceCollection services, string serviceKey, Action<CaseROptions>? configure = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(serviceKey, nameof(serviceKey));

        CaseROptions options = new CaseROptions(serviceKey);
        configure?.Invoke(options);

        services.TryAddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.TryAddScoped<IDomainEventPublisherStrategy, ForeachAwaitPublisherStrategy>();

        services.AddKeyedScoped(typeof(IUseCase<>), serviceKey, typeof(KeyedUseCase<>));
        services.AddKeyedTransient(typeof(IAutoScopedUseCase<>), serviceKey, typeof(KeyedAutoScopedUseCase<>));

        options.Register(services);
    }
}
