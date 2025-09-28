namespace CaseR.Tests.Interceptors;

public class OtherStreamIntecerptor<TRequest, TResponse> : IUseCaseStreamInterceptor<TRequest, TResponse>
{
    private readonly CallAssertion assertion;

    public OtherStreamIntecerptor(CallAssertion assertion)
    {
        this.assertion = assertion;
    }

    public async IAsyncEnumerable<TResponse> InterceptExecution(IUseCaseStreamInteractor<TRequest, TResponse> useCaseInteractor, TRequest request, UseCaseStreamPerformDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        IAsyncEnumerable<TResponse> response = next(request);
        this.assertion.AddCall("OtherStreamIntecerptor");

        await foreach (TResponse item in response)
        {
            yield return item;
        }
    }
}