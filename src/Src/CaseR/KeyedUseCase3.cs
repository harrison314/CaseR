using Microsoft.Extensions.DependencyInjection;

namespace CaseR;

internal class KeyedUseCase3<TInteractor, TRequest, TResponse> : IUseCase<TInteractor, TRequest, TResponse>
    where TInteractor : IUseCaseInteractor<TRequest, TResponse>
{
    private readonly TInteractor useCase;
    private readonly IServiceProvider serviceProvider;
    private readonly string instanceKey;

    public KeyedUseCase3(TInteractor useCase, IServiceProvider serviceProvider, [ServiceKey] string instanceKey)
    {
        this.useCase = useCase;
        this.serviceProvider = serviceProvider;
        this.instanceKey = instanceKey;
    }

    public async ValueTask<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default)
    {
        IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>? middlewares = this.serviceProvider.GetKeyedService<IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>>(this.instanceKey);
        if (middlewares is null || !middlewares.Any())
        {
            return await this.useCase.Execute(request, cancellationToken).ConfigureAwait(false);
        }

        UseCasePerformDelegate<TRequest, TResponse> next = (req) => this.useCase.Execute(req, cancellationToken);

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