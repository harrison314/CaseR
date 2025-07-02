namespace CaseR;

public delegate Task<TResponse> UseCasePerformDelegate<TRequest, TResponse>(TRequest request);

public interface IUseCaseInterceptor<TRequest, TResponse>
{
    Task<TResponse> InterceptExecution(IUseCaseInteractor<TRequest, TResponse> useCaseInteractor,
        TRequest request,
        UseCasePerformDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken);
}