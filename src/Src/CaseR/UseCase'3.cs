using Microsoft.Extensions.DependencyInjection;

namespace CaseR;

internal class UseCase3<TInteractor, TRequest, TResponse> : IUseCase<TInteractor, TRequest, TResponse>
    where TInteractor : IUseCaseInteractor<TRequest, TResponse>
{
    private readonly TInteractor useCase;
    private readonly IServiceProvider serviceProvider;

    public UseCase3(TInteractor useCase, IServiceProvider serviceProvider)
    {
        this.useCase = useCase;
        this.serviceProvider = serviceProvider;
    }

    public async ValueTask<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default)
    {
        IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>? middlewares = this.serviceProvider.GetService<IEnumerable<IUseCaseInterceptor<TRequest, TResponse>>>();
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
                IUseCaseInterceptor<TRequest, TResponse> interceptor = array[i];
                next = async (req) => await interceptor.InterceptExecution(this.useCase, req, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        else if (middlewares is List<IUseCaseInterceptor<TRequest, TResponse>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                UseCasePerformDelegate<TRequest, TResponse> currentNext = next;
                IUseCaseInterceptor<TRequest, TResponse> interceptor = list[i];
                next = async (req) => await interceptor.InterceptExecution(this.useCase, req, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        else
        {
            foreach (IUseCaseInterceptor<TRequest, TResponse> middleware in middlewares)
            {
                UseCasePerformDelegate<TRequest, TResponse> currentNext = next;
                next = async (req) => await middleware.InterceptExecution(this.useCase, req, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        return await next(request).ConfigureAwait(false);
    }
}
