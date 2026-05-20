namespace CaseR.Tests.Interactors;

public record BackgroundJobRequest();
public record BackgroundJobResponse();

[RegistrationCathegory("BackgroundJob")]
internal class BackgroundJobInteractor : IUseCaseInteractor<BackgroundJobRequest, BackgroundJobResponse>
{
    public Task<BackgroundJobResponse> Execute(BackgroundJobRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new BackgroundJobResponse());
    }
}
