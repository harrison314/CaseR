namespace CaseR.Tests.EventHandlers;

public class Bar2EventHandler : IDomainEventHandler<BarEvent>
{
    private readonly CallAssertion callAssertion;

    public Bar2EventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public Task Handle(BarEvent @event, CancellationToken cancellationToken)
    {

        this.callAssertion.AddCall("Bar2EventHandler");
        return Task.CompletedTask;
    }
}
