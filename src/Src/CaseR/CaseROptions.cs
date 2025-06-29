using CaseR;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

public sealed class CaseROptions
{
    private List<ServiceDescriptor> interceptors;
    private readonly string? serviceKey;

    internal CaseROptions(string? serviceKey)
    {
        this.interceptors = new List<ServiceDescriptor>();
        this.serviceKey = serviceKey;
    }

    public void AddGenericInterceptor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type interceptorType)
    {
        ArgumentNullException.ThrowIfNull(interceptorType);
        System.Diagnostics.Debug.Assert(interceptorType.IsGenericType);

        this.interceptors.Add(new ServiceDescriptor(typeof(IUseCaseInterceptor<,>), this.serviceKey, interceptorType, ServiceLifetime.Scoped));
    }

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