namespace CaseR.Tests.Interactors;

[ExcludeFromRegistration]
public class ExcludeUseCaseStreamInteractor : IUseCaseStreamInteractor<Unit, Unit>
{
    public ExcludeUseCaseStreamInteractor()
    {

    }

    public async IAsyncEnumerable<Unit> Execute(Unit request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        yield return Unit.Value;
    }
}