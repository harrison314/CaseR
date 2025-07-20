namespace CaseR;

/// <summary>
/// Defines a contract for implementig use case interactor.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
/// <seealso cref="Unit" />
public interface IUseCaseInteractor<in TRequest, TResponse> : IUseCaseInteractorBase
{
    /// <summary>
    /// Executes the use case interactor.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation should be canceled if the token is triggered</param>
    /// <returns>Interactor response.</returns>
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
}