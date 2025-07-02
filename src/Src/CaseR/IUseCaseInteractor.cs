namespace CaseR;

public interface IUseCaseInteractor<in TRequest, TResponse> : IUseCaseInteractorBase
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
}