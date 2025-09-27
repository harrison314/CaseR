using CaseR.Tests.Interactors;
using CaseR.Tests.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace CaseR.Tests;

[TestClass]
public sealed class PipelinesTests
{
    [TestMethod]
    public async Task AddCaseR_NoInteceptor_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute<PingPongInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);

        callAssertion.AssertNoCalls();
    }

    [TestMethod]
    public async Task AddCaseR_StreamingNoInteceptor_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongStreamingInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongStreamingInteractor>>();

        IAsyncEnumerable<Pong> pongs = interactor.ExecuteStreaming(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pongs);

        callAssertion.AssertNoCalls();
    }

    [TestMethod]
    public async Task AddCaseR_GenericInteceptor_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR(options =>
        {
            options.AddGenericInterceptor(typeof(CallIntecerptor<,>));
        });

        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute<PingPongInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);

        callAssertion.AssertCall("CallIntecerptor");
    }

    [TestMethod]
    public async Task AddCaseR_ConcereteInteceptor_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR(options =>
        {
            options.AddInterceptor<Ping, Pong, ConcereteInterceptor>();
        });

        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute<PingPongInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);

        callAssertion.AssertCall("ConcereteInterceptor");
    }

    [TestMethod]
    public async Task AddCaseR_MultipleInteceptors_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR(options =>
        {
            options.AddGenericInterceptor(typeof(CallIntecerptor<,>));
            options.AddInterceptor<Ping, Pong, ConcereteInterceptor>();
        });

        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute<PingPongInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);

        callAssertion.AssertCall("ConcereteInterceptor");
        callAssertion.AssertCall("CallIntecerptor");
    }
}
