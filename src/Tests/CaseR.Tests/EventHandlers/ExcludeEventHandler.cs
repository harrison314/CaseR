using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests.EventHandlers;

[ExcludeFromRegistration]
public class ExcludeEventHandler : IDomainEventHandler<ExcludeEvent>
{
    public ExcludeEventHandler()
    {

    }

    public Task Handle(ExcludeEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
