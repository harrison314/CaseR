using System.Runtime.CompilerServices;

namespace CaseR.Tests.Interactors;

[ExcludeFromRegistration]
public class ExcludeUseCaseStreamInteractor : IUseCaseStreamInteractor<Unit, Unit>
{
    public ExcludeUseCaseStreamInteractor()
    {

    }

    public async IAsyncEnumerable<Unit> Execute(Unit request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        yield return Unit.Value;
    }
}