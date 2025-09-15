using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

internal class AutoScopedUseCase<T> : IAutoScopedUseCase<T>
    where T : IUseCaseInteractorBase
{
    private readonly IServiceProvider serviceProvider;

    public AutoScopedUseCase(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    async Task<TResponse> IUseCase<T>.InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        IServiceScopeFactory scopeFactory = this.serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope scope = scopeFactory.CreateScope();
        IUseCase<T> useCase = scope.ServiceProvider.GetRequiredService<IUseCase<T>>();

        return await useCase.InternalExecute<TRequest, TResponse>(request, cancellationToken).ConfigureAwait(false);
    }

    async IAsyncEnumerable<TResponse> IUseCase<T>.InternalExecuteStreaming<TRequest, TResponse>(TRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IServiceScopeFactory scopeFactory = this.serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope scope = scopeFactory.CreateScope();
        IUseCase<T> useCase = scope.ServiceProvider.GetRequiredService<IUseCase<T>>();

        await foreach (TResponse response in useCase.InternalExecuteStreaming<TRequest, TResponse>(request, cancellationToken))
        {
            yield return response;
        }
    }
}
