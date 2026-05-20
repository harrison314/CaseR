namespace CaseR.Tests.EventHandlers;

[RegistrationCathegory("Api")]
public class BarApiEventHandler : IDomainEventHandler<BarEvent>
{
    private readonly CallAssertion callAssertion;

    public BarApiEventHandler(CallAssertion callAssertion)
    {
        this.callAssertion = callAssertion;
    }

    public Task Handle(BarEvent @event, CancellationToken cancellationToken)
    {

        this.callAssertion.AddCall("BarApiEventHandler");
        return Task.CompletedTask;
    }
}