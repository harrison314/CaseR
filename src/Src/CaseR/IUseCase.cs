namespace CaseR;

/// <summary>
/// Represents a use case that defines a contract for executing a specific interactor.
/// </summary>
/// <typeparam name="T">The type of the interactor that implements the use case logic.</typeparam>
public interface IUseCase<T>
    where T : IUseCaseInteractorBase
{
    internal Task<TResponse> InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
}
