using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

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

    async IAsyncEnumerable<TResponse> IUseCase<T>.InternalExecuteStreaming<TRequest, TResponse>(TRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IServiceScopeFactory scopeFactory = this.serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope scope = scopeFactory.CreateScope();
        IUseCase<T> useCase = scope.ServiceProvider.GetRequiredKeyedService<IUseCase<T>>(this.instanceKey);

        await foreach (TResponse response in useCase.InternalExecuteStreaming<TRequest, TResponse>(request, cancellationToken))
        {
            yield return response;
        }
    }
}