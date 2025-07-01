using CaseR.Tests.Interactors;

namespace CaseR.Tests.Interceptors;

public class ConcereteInterceptor : IUseCaseInterceptor<Ping, Pong>
{
    private readonly CallAssertion assertion;

    public ConcereteInterceptor(CallAssertion assertion)
    {
        this.assertion = assertion;
    }

    public async ValueTask<Pong> InterceptExecution(IUseCaseInteractor<Ping, Pong> useCaseInteractor, Ping request, UseCasePerformDelegate<Ping, Pong> next, CancellationToken cancellationToken)
    {
        Pong response = await next(request);
        this.assertion.AddCall("ConcereteInterceptor");
        return response;
    }
}
