using CaseR.Tests.EventHandlers;
using CaseR.Tests.Interactors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests;

[TestClass]
public sealed class EventHandlersTests
{
    [TestMethod]
    public async Task DomainEvents_RegisterAndUseWithReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IDomainEventPublisher publisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();

       await publisher.Publish(new FooEvent("FooEvent"));

        callAssertion.AssertCall("FooEventHandler");
    }

    [TestMethod]
    public async Task DomainEvents_RegisterAndUseWithGeneration_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IDomainEventPublisher publisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();

        await publisher.Publish(new FooEvent("FooEvent"));

        callAssertion.AssertCall("FooEventHandler");
    }

    [TestMethod]
    public async Task DomainEvents_MiltipleHandlersReflectionRegistration_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(BarEvent));

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IDomainEventPublisher publisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();

        await publisher.Publish(new BarEvent("BarEvent"));

        callAssertion.AssertCall("BarEventHandler");
        callAssertion.AssertCall("Bar2EventHandler");
        callAssertion.AssertCall("Bar3EventHandler");
    }

    [TestMethod]
    public async Task DomainEvents_MiltipleHandlersSourceGenerator_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        CallAssertion callAssertion = new CallAssertion();
        serviceCollection.AddSingleton(callAssertion);

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IDomainEventPublisher publisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();

        await publisher.Publish(new BarEvent("BarEvent"));

        callAssertion.AssertCall("BarEventHandler");
        callAssertion.AssertCall("Bar2EventHandler");
        callAssertion.AssertCall("Bar3EventHandler");
    }


    [TestMethod]
    [Ignore]
    public async Task DomainEvents_GenericHandlerReflectionRegistration_Success()
    {
        throw new NotImplementedException("Implement generic handlers registration");
    }

    [TestMethod]
    [Ignore]
    public async Task DomainEvents_GenericHandlerSourceGenerator_Success()
    {
        throw new NotImplementedException("Implement generic handlers registration");
    }
}
