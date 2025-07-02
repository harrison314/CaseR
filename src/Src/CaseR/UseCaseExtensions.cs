using System.Runtime.CompilerServices;

namespace CaseR;

public static class UseCaseExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TResponse> Execute<TInteractor, TRequest, TResponse>(
        this IUseCase<TInteractor> useCase,
        TRequest request,
        CancellationToken cancellationToken = default)
        where TInteractor : IUseCaseInteractor<TRequest, TResponse>
    {
        return await useCase.InternalExecute<TRequest, TResponse>(request, cancellationToken)
            .ConfigureAwait(false);
    }
}
