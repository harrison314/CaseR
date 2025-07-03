using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
