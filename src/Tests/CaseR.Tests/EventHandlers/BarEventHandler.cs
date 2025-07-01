namespace CaseR.Tests.EventHandlers;

public class BarEventHandler : IDomainEventHandler<BarEvent>
{
    private readonly CallAssertion callAssertion;

    public BarEventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public ValueTask Handle(BarEvent @event, CancellationToken cancellationToken)
    {

        this.callAssertion.AddCall("BarEventHandler");
        return ValueTask.CompletedTask;
    }
}
