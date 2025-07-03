namespace CaseR;

public interface IUseCase<T>
    where T : IUseCaseInteractorBase
{
    internal Task<TResponse> InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
}


