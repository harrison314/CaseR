using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests.EventHandlers;

public class GenericEventHandler<T> : IDomainEventHandler<T>
    where T : IDomainEvent
{
    private readonly CallAssertion callAssertion;

    public GenericEventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public ValueTask Handle(T domainEvent, CancellationToken cancellationToken)
    {
        this.callAssertion.AddCall("GenericEventHandler");
        return ValueTask.CompletedTask;
    }
}
