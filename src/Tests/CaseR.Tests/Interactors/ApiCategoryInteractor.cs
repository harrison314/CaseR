namespace CaseR.Tests.Interactors;

public record ApiRequest();
public record ApiResponse();

[RegistrationCathegory("Api")]
internal class ApiCategoryInteractor : IUseCaseInteractor<ApiRequest, ApiResponse>
{
    public Task<ApiResponse> Execute(ApiRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ApiResponse());
    }
}
