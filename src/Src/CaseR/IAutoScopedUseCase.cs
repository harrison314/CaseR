namespace CaseR;

public interface IAutoScopedUseCase<T> : IUseCase<T>
    where T : IUseCaseInteractorBase
{
}