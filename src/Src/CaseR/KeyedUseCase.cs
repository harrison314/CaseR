using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace CaseR;

internal class KeyedUseCase<TInteractor> : IUseCase<TInteractor>
    where TInteractor : IUseCaseInteractorBase
{
    private readonly TInteractor useCase;
    private readonly IServiceProvider serviceProvider;
    private readonly string instanceKey;

    public KeyedUseCase(TInteractor useCase, IServiceProvider serviceProvider, [ServiceKey] string instanceKey)
    {
        this.useCase = useCase;
        this.serviceProvider = serviceProvider;
        this.instanceKey = instanceKey;
    }

    async ValueTask<TResponse> IUseCase<TInteractor>.InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        return await this.InvokeWithPipeline(Unsafe.As<IUseCaseInteractor<TRequest, TResponse>>(this.useCase),
            request,
            cancellationToken)
            .ConfigureAwait(false);
    }

    private async ValueTask<TResponse> InvokeWithPipeline<TRequest, TResponse>(IUseCaseInteractor<TRequest, TResponse> typedUseCase,
        TRequest request,
        CancellationToken cancellationToken)
    {
        IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>? middlewares = this.serviceProvider.GetKeyedService<IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>>(this.instanceKey);
        if (middlewares is null || !middlewares.Any())
        {
            ValueTask<TResponse> valueTask = typedUseCase.Execute(request, cancellationToken);
            if (!valueTask.IsCompletedSuccessfully)
            {
                return await valueTask.ConfigureAwait(false);
            }

            return valueTask.Result;
        }

        UseCasePerformDelegate<TRequest, TResponse> next = (req) => typedUseCase.Execute(req, cancellationToken);

        if (middlewares is IUseCaseInterceptor<TRequest, TResponse>[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                UseCasePerformDelegate<TRequest, TResponse> currentNext = next;
                next = async (req) => await array[i].InterceptExecution(typedUseCase, req, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        else if (middlewares is List<IUseCaseInterceptor<TRequest, TResponse>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                UseCasePerformDelegate<TRequest, TResponse> currentNext = next;
                next = async (req) => await list[i].InterceptExecution(typedUseCase, req, currentNext, cancellationToken)
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

