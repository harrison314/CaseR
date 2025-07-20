using System.Runtime.CompilerServices;

namespace CaseR;

/// <summary>
/// Use case public extensions.
/// </summary>
public static class UseCaseExtensions
{

    /// <summary>
    /// Execute the use case interactor.
    /// </summary>
    /// <typeparam name="TInteractor">Use case interactor type.</typeparam>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="useCase">Use case interactor.</param>
    /// <param name="request">Request instance.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>Returns task with result.</returns>
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
