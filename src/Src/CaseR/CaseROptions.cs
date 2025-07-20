using CaseR;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Options for configuring CaseR.
/// </summary>
public sealed class CaseROptions
{
    private List<ServiceDescriptor> interceptors;
    private readonly string? serviceKey;

    internal CaseROptions(string? serviceKey)
    {
        this.interceptors = new List<ServiceDescriptor>();
        this.serviceKey = serviceKey;
    }

    /// <summary>
    /// Add interceptor with open generic type.
    /// </summary>
    /// <param name="interceptorType">Open generic interceptor type.</param>
    public void AddGenericInterceptor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type interceptorType)
    {
        ArgumentNullException.ThrowIfNull(interceptorType);
        if (!interceptorType.IsGenericType)
        {
            throw new ArgumentException("Interceptor type must be an open generic type.", nameof(interceptorType));
        }

        this.interceptors.Add(new ServiceDescriptor(typeof(IUseCaseInterceptor<,>), this.serviceKey, interceptorType, ServiceLifetime.Scoped));
    }

    /// <summary>
    /// Add concrete interceptor type.
    /// </summary>
    /// <typeparam name="TRequest">Type for interactor request.</typeparam>
    /// <typeparam name="TResponse">Type for interactor response.</typeparam>
    /// <typeparam name="TInterceptor">Interceptor type.</typeparam>
    public void AddInterceptor<TRequest, TResponse, TInterceptor>()
        where TInterceptor : IUseCaseInterceptor<TRequest, TResponse>
    {
        this.interceptors.Add(new ServiceDescriptor(typeof(IUseCaseInterceptor<TRequest, TResponse>), this.serviceKey, typeof(TInterceptor), ServiceLifetime.Scoped));
    }

    internal void Register(IServiceCollection services)
    {
        foreach (ServiceDescriptor type in this.interceptors.Reverse<ServiceDescriptor>())
        {
            services.Add(type);
        }
    }
}