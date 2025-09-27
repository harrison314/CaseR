using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests.Interactors;

public record Ping();
public record Pong();

internal class PingPongInteractor : IUseCaseInteractor<Ping, Pong>
{
    public PingPongInteractor()
    {

    }

    public Task<Pong> Execute(Ping request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Pong());
    }
}
