using CaseR.Tests.Interactors;
using Microsoft.Extensions.DependencyInjection;

namespace CaseR.Tests;

[TestClass]
public sealed class RegistrationTests
{
    [TestMethod]
    public async Task AddCaseR_Registration_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();
    }

    [TestMethod]
    public async Task AddCaseR_RegisterAndUseWithReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute<PingPongInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);
    }

    [TestMethod]
    public async Task AddCaseR_RegisterAndUseWithGeneration_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);
    }
}
