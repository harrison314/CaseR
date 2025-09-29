using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests.Interactors;

[ExcludeFromRegistration]
public class ExcludeUseCaseInteractor : IUseCaseInteractor<Unit, Unit>
{
    public ExcludeUseCaseInteractor()
    {

    }

    public Task<Unit> Execute(Unit request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }
}
