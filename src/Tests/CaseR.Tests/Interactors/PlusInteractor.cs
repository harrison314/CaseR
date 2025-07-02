namespace CaseR.Tests.Interactors;

internal class PlusInteractor : IUseCaseInteractor<PlusInteractorRequest, int>
{
    public PlusInteractor()
    {

    }

    public Task<int> Execute(PlusInteractorRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.A + request.B);
    }
}