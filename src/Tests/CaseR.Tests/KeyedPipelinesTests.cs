﻿using CaseR.Tests.Interactors;
using CaseR.Tests.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace CaseR.Tests;

[TestClass]
public sealed class KeyedPipelinesTests
{
    [TestMethod]
    public async Task KeyedPipeline_GenericInteceptor_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR(options =>
        {
            options.AddGenericInterceptor(typeof(OtherIntecerptor<,>));
        });

        serviceCollection.AddKeyedCaseR("Keyed", options =>
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

        callAssertion.AssertCall("OtherIntecerptor");
    }

    [TestMethod]
    public async Task KeyedPipeline_KeyedInteceptor_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR(options =>
        {
            options.AddGenericInterceptor(typeof(OtherIntecerptor<,>));
        });

        serviceCollection.AddKeyedCaseR("Keyed", options =>
        {
            options.AddGenericInterceptor(typeof(CallIntecerptor<,>));
        });

        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredKeyedService<IUseCase<PingPongInteractor>>("Keyed");

        Pong pong = await interactor.Execute<PingPongInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);

        callAssertion.AssertCall("CallIntecerptor");
    }

    [TestMethod]
    public async Task KeyedPipeline_Includerelation_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR(options =>
        {
            options.AddGenericInterceptor(typeof(OtherIntecerptor<,>));
        });

        serviceCollection.AddKeyedCaseR(UseCaseRelation.Include, options =>
        {
            options.AddGenericInterceptor(typeof(CallIntecerptor<,>));
        });

        serviceCollection.AddCaseRInteractors();

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<MathInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<MathInteractor>>();

        int result = await interactor.Execute(new MathInteractorRequest(3, 2, 5), CancellationToken.None);

        Assert.AreEqual(25, result);
    }
}