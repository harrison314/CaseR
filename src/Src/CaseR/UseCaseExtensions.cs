using System.Runtime.CompilerServices;

namespace CaseR;

public static class UseCaseExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<TResponse> Execute<TInteractor, TRequest, TResponse>(
        this IUseCase<TInteractor> useCase,
        TRequest request,
        CancellationToken cancellationToken = default)
        where TInteractor : IUseCaseInteractor<TRequest, TResponse>
    {
        return useCase.InternalExecute<TRequest, TResponse>(request, cancellationToken);
    }
}
