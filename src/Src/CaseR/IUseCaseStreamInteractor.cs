namespace CaseR;

public interface IUseCaseStreamInteractor<in TRequest, TResponse> : IUseCaseInteractorBase
{
    /// <summary>
    /// Executes the use case interactor.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation should be canceled if the token is triggered</param>
    /// <returns>Interactor response.</returns>
    IAsyncEnumerable<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
}