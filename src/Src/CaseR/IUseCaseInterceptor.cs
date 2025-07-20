namespace CaseR;

/// <summary>
/// Defines a mechanism to intercept the execution of a use case, allowing custom logic to be applied before or after
/// the use case is executed.
/// </summary>
/// <typeparam name="TRequest">The type of the request object passed to the use case.</typeparam>
/// <typeparam name="TResponse">The type of the response object returned by the use case.</typeparam>
public interface IUseCaseInterceptor<TRequest, TResponse>
{
    /// <summary>
    /// Method to intercept the execution of a use case interactor.
    /// </summary>
    /// <param name="useCaseInteractor">The interactor instance.</param>
    /// <param name="request">The request instance.</param>
    /// <param name="next">The next delegate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation should be canceled if the token is triggered</param>
    /// <returns>Interactor response.</returns>
    Task<TResponse> InterceptExecution(IUseCaseInteractor<TRequest, TResponse> useCaseInteractor,
        TRequest request,
        UseCasePerformDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken);
}