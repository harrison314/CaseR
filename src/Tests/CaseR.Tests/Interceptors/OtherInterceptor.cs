namespace CaseR.Tests.Interceptors;

public class OtherInterceptor<TRequest, TResponse> : IUseCaseInterceptor<TRequest, TResponse>
{
    private readonly CallAssertion assertion;

    public OtherInterceptor(CallAssertion assertion)
    {
        this.assertion = assertion;
    }

    public async Task<TResponse> InterceptExecution(IUseCaseInteractor<TRequest, TResponse> useCaseInteractor, TRequest request, UseCasePerformDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = await next(request);
        this.assertion.AddCall("OtherInterceptor");

        return response;
    }
}