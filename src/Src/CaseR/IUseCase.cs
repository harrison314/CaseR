namespace CaseR;

public interface IUseCase<T>
    where T : IUseCaseInteractorBase
{
    internal Task<TResponse> InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
}

public interface IUseCase<TInteracror, in TRequest, TResponse>
    where TInteracror : IUseCaseInteractor<TRequest, TResponse>
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
}


