namespace CaseR;

public interface IUseCase<T>
    where T : IUseCaseInteractorBase
{
    internal ValueTask<TResponse> InternalExecute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
}

public interface IUseCase<TInteracror, in TRequest, TResponse>
    where TInteracror : IUseCaseInteractor<TRequest, TResponse>
{
    ValueTask<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
}


