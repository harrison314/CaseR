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

    IAsyncEnumerable<TResponse> IUseCase<TInteractor>.InternalExecuteStreaming<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        IUseCaseStreamInteractor<TRequest, TResponse> typedUseCase = (IUseCaseStreamInteractor<TRequest, TResponse>)this.useCase;

        IEnumerable<IUseCaseStreamInterceptor<TRequest, TResponse>>? middlewares = this.serviceProvider.GetService<IEnumerable<IUseCaseStreamInterceptor<TRequest, TResponse>>>();
        if (middlewares is null || !middlewares.Any())
        {
            return typedUseCase.Execute(request, cancellationToken);
        }

        UseCaseStreamPerformDelegate<TRequest, TResponse> next = (req) => typedUseCase.Execute(req, cancellationToken);

        if (middlewares is IUseCaseStreamInterceptor<TRequest, TResponse>[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                UseCaseStreamPerformDelegate<TRequest, TResponse> currentNext = next;
                IUseCaseStreamInterceptor<TRequest, TResponse> interceptor = array[i];
                next = (req) => interceptor.InterceptExecution(typedUseCase, req, currentNext, cancellationToken);
            }
        }
        else if (middlewares is List<IUseCaseStreamInterceptor<TRequest, TResponse>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                UseCaseStreamPerformDelegate<TRequest, TResponse> currentNext = next;
                IUseCaseStreamInterceptor<TRequest, TResponse> interceptor = list[i];
                next = (req) => interceptor.InterceptExecution(typedUseCase, req, currentNext, cancellationToken);
            }
        }
        else
        {
            foreach (IUseCaseStreamInterceptor<TRequest, TResponse> middleware in middlewares)
            {
                UseCaseStreamPerformDelegate<TRequest, TResponse> currentNext = next;
                next = (req) => middleware.InterceptExecution(typedUseCase, req, currentNext, cancellationToken);
            }
        }

        return next(request);
    }
}
