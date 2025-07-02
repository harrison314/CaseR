namespace CaseR.Tests.EventHandlers;

public class BarEventHandler : IDomainEventHandler<BarEvent>
{
    private readonly CallAssertion callAssertion;

    public BarEventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public Task Handle(BarEvent @event, CancellationToken cancellationToken)
    {

        this.callAssertion.AddCall("BarEventHandler");
        return Task.CompletedTask;
    }
}
