namespace CaseR;

/// <summary>
/// Defines a mechanism to intercept the execution of a use case, allowing custom logic to be applied before or after
/// the use case is executed.
/// </summary>
/// <typeparam name="TRequest">The type of the request object passed to the use case.</typeparam>
/// <typeparam name="TResponse">The type of streaming response object returned by the use case.</typeparam>
public interface IUseCaseStreamInterceptor<TRequest, TResponse>
{
    /// <summary>
    /// Method to intercept the execution of a streaming use case interactor.
    /// </summary>
    /// <param name="useCaseInteractor">The interactor instance.</param>
    /// <param name="request">The request instance.</param>
    /// <param name="next">The next delegate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation should be canceled if the token is triggered.</param>
    /// <returns>Iterator streaming response.</returns>
    IAsyncEnumerable<TResponse> InterceptExecution(IUseCaseStreamInteractor<TRequest, TResponse> useCaseInteractor,
        TRequest request,
        UseCaseStreamPerformDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken);
}