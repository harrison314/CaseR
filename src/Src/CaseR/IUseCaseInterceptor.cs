namespace CaseR;

public delegate ValueTask<TResponse> UseCasePerformDelegate<TRequest, TResponse>(TRequest request);

public interface IUseCaseInterceptor<TRequest, TResponse>
{
    ValueTask<TResponse> InterceptExecution(IUseCaseInteractor<TRequest, TResponse> useCaseInteractor,
        TRequest request,
        UseCasePerformDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken);
}