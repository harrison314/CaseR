namespace CaseR.Tests.Interactors;

internal class PlusInteractor : IUseCaseInteractor<PlusInteractorRequest, int>
{
    public PlusInteractor()
    {

    }

    public ValueTask<int> Execute(PlusInteractorRequest request, CancellationToken cancellationToken)
    {
        return new ValueTask<int>(request.A + request.B);
    }
}