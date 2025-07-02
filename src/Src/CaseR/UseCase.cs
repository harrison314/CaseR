using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace CaseR;

internal class UseCase<TInteractor> : IUseCase<TInteractor>
    where TInteractor : IUseCaseInteractorBase
{
    private readonly TInteractor useCase;
    private readonly IServiceProvider serviceProvider;

    public UseCase(TInteractor useCase, IServiceProvider serviceProvider)
    {
        this.useCase = useCase;
        this.serviceProvider = serviceProvider;
    }

    async Task<TResponse> IUseCase<TInteractor>.InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        IUseCaseInteractor<TRequest, TResponse> typedUseCase = (IUseCaseInteractor<TRequest, TResponse>)this.useCase;

        IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>? middlewares = this.serviceProvider.GetService<IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>>();
        if (middlewares is null || !middlewares.Any())
        {
            return await typedUseCase.Execute(request, cancellationToken).ConfigureAwait(false);
        }

        UseCasePerformDelegate<TRequest, TResponse> next = (req) => typedUseCase.Execute(req, cancellationToken);

        if (middlewares is IUseCaseInterceptor<TRequest, TResponse>[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                UseCasePerformDelegate<TRequest, TResponse> currentNext = next;
                IUseCaseInterceptor<TRequest, TResponse> interceptor = array[i];
                next = async (req) => await interceptor.InterceptExecution(typedUseCase, req, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        else if (middlewares is List<IUseCaseInterceptor<TRequest, TResponse>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                UseCasePerformDelegate<TRequest, TResponse> currentNext = next;
                IUseCaseInterceptor<TRequest, TResponse> interceptor = list[i];
                next = async (req) => await interceptor.InterceptExecution(typedUseCase, req, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        else
        {
            foreach (IUseCaseInterceptor<TRequest, TResponse> middleware in middlewares)
            {
                UseCasePerformDelegate<TRequest, TResponse> currentNext = next;
                next = async (req) => await middleware.InterceptExecution(typedUseCase, req, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        return await next(request).ConfigureAwait(false);
    }
}
