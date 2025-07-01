namespace CaseR.Tests.Interceptors;

public class OtherIntecerptor<TRequest, TResponse> : IUseCaseInterceptor<TRequest, TResponse>
{
    private readonly CallAssertion assertion;

    public OtherIntecerptor(CallAssertion assertion)
    {
        this.assertion = assertion;
    }

    public async ValueTask<TResponse> InterceptExecution(IUseCaseInteractor<TRequest, TResponse> useCaseInteractor, TRequest request, UseCasePerformDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = await next(request);
        this.assertion.AddCall("OtherIntecerptor");

        return response;
    }
}