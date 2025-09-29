using System.Runtime.CompilerServices;

namespace CaseR.Tests.Interceptors;

public class CallStreamIntecerptor<TRequest, TResponse> : IUseCaseStreamInterceptor<TRequest, TResponse>
{
    private readonly CallAssertion assertion;

    public CallStreamIntecerptor(CallAssertion assertion)
    {
        this.assertion = assertion;
    }

    public async IAsyncEnumerable<TResponse> InterceptExecution(IUseCaseStreamInteractor<TRequest, TResponse> useCaseInteractor, TRequest request, UseCaseStreamPerformDelegate<TRequest, TResponse> next, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<TResponse> response = next(request);
        this.assertion.AddCall("CallStreamIntecerptor");

        await foreach (TResponse item in response)
        {
            yield return item;
        }
    }
}
