namespace CaseR;

public interface IUseCaseInteractor<in TRequest, TResponse> : IUseCaseInteractorBase
{
    ValueTask<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
}