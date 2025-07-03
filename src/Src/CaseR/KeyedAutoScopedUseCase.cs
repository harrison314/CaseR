using Microsoft.Extensions.DependencyInjection;

namespace CaseR;

internal class KeyedAutoScopedUseCase<T> : IAutoScopedUseCase<T>
    where T : IUseCaseInteractorBase
{
    private readonly IServiceProvider serviceProvider;
    private readonly string instanceKey;

    public KeyedAutoScopedUseCase(IServiceProvider serviceProvider, [ServiceKey] string instanceKey)
    {
        this.serviceProvider = serviceProvider;
        this.instanceKey = instanceKey;
    }

    async Task<TResponse> IUseCase<T>.InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        IServiceScopeFactory scopeFactory = this.serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope scope = scopeFactory.CreateScope();
        IUseCase<T> useCase = scope.ServiceProvider.GetRequiredKeyedService<IUseCase<T>>(this.instanceKey);

        return await useCase.InternalExecute<TRequest, TResponse>(request, cancellationToken).ConfigureAwait(false);
    }
}