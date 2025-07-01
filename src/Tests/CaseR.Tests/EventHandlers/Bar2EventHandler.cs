namespace CaseR.Tests.EventHandlers;

public class Bar2EventHandler : IDomainEventHandler<BarEvent>
{
    private readonly CallAssertion callAssertion;

    public Bar2EventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public ValueTask Handle(BarEvent @event, CancellationToken cancellationToken)
    {

        this.callAssertion.AddCall("Bar2EventHandler");
        return ValueTask.CompletedTask;
    }
}
