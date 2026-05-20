namespace CaseR.Tests.Interactors;

public record MultiCategoryRequest();
public record MultiCategoryResponse();

[RegistrationCathegory("Api")]
[RegistrationCathegory("BackgroundJob")]
internal class MultiCategoryInteractor : IUseCaseInteractor<MultiCategoryRequest, MultiCategoryResponse>
{
    public Task<MultiCategoryResponse> Execute(MultiCategoryRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new MultiCategoryResponse());
    }
}
