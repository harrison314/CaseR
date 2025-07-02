using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests.Interceptors;

public class CallIntecerptor<TRequest, TResponse> : IUseCaseInterceptor<TRequest, TResponse>
{
    private readonly CallAssertion assertion;

    public CallIntecerptor(CallAssertion assertion)
    {
        this.assertion = assertion;
    }

    public async Task<TResponse> InterceptExecution(IUseCaseInteractor<TRequest, TResponse> useCaseInteractor, TRequest request, UseCasePerformDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = await next(request);
        this.assertion.AddCall("CallIntecerptor");

        return response;
    }
}
