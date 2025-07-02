namespace CaseR.Tests.EventHandlers;

public class FooEventHandler : IDomainEventHandler<FooEvent>
{
    private readonly CallAssertion callAssertion;

    public FooEventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public Task Handle(FooEvent @event, CancellationToken cancellationToken)
    {

        this.callAssertion.AddCall("FooEventHandler");
        return Task.CompletedTask;
    }
}
