namespace CaseR.Tests.EventHandlers;

public class Bar3EventHandler : IDomainEventHandler<BarEvent>
{
    private readonly CallAssertion callAssertion;

    public Bar3EventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public Task Handle(BarEvent @event, CancellationToken cancellationToken)
    {

        this.callAssertion.AddCall("Bar3EventHandler");
        return Task.CompletedTask;
    }
}