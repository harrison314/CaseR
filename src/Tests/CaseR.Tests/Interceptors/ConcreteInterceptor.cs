using CaseR.Tests.Interactors;

namespace CaseR.Tests.Interceptors;

public class ConcreteInterceptor : IUseCaseInterceptor<Ping, Pong>
{
    private readonly CallAssertion assertion;

    public ConcreteInterceptor(CallAssertion assertion)
    {
        this.assertion = assertion;
    }

    public async Task<Pong> InterceptExecution(IUseCaseInteractor<Ping, Pong> useCaseInteractor, Ping request, UseCasePerformDelegate<Ping, Pong> next, CancellationToken cancellationToken)
    {
        Pong response = await next(request);
        this.assertion.AddCall("ConcreteInterceptor");
        return response;
    }
}
