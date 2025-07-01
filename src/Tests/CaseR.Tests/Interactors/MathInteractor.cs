using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests.Interactors;

internal class MathInteractor : IUseCaseInteractor<MathInteractorRequest, int>
{
    private readonly IUseCase<PlusInteractor> plusInteractors;

    public MathInteractor([FromKeyedServices(UseCaseRelation.Include)] IUseCase<PlusInteractor> plusInteractors)
    {
        this.plusInteractors = plusInteractors;
    }

    public async ValueTask<int> Execute(MathInteractorRequest request, CancellationToken cancellationToken)
    {
        int sum = await this.plusInteractors.Execute(new PlusInteractorRequest(request.A, request.B), cancellationToken);

        return sum * request.C;
    }
}
