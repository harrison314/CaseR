namespace CaseR;

/// <summary>
/// Represents a use case that is automatically scoped lifetime and operates on a specified type.
/// </summary>
/// <typeparam name="T">The type of the interactor that this use case operates on.</typeparam>
public interface IAutoScopedUseCase<T> : IUseCase<T>
    where T : IUseCaseInteractorBase
{
}