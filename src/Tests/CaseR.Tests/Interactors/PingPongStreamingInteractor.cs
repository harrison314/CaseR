using System.Runtime.CompilerServices;

namespace CaseR.Tests.Interactors;

internal class PingPongStreamingInteractor : IUseCaseStreamInteractor<Ping, Pong>
{
    public PingPongStreamingInteractor()
    {

    }

    public async IAsyncEnumerable<Pong> Execute(Ping request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        yield return new Pong();
        yield return new Pong();
        await Task.Delay(0, cancellationToken);
        yield return new Pong();
    }
}