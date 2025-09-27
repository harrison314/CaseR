using CaseR.Tests.EventHandlers;
using CaseR.Tests.Interactors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests;

[TestClass]
public sealed class ExcludeFromRegistrationTests
{
    [TestMethod]
    public async Task ExcludeFromRegistration_RegisterInteractorWithReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(ExcludeFromRegistrationTests));


        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        object? interactor = scope.ServiceProvider.GetService(typeof(ExcludeUseCaseInteractor));

        Assert.IsNull(interactor, "ExcludeFromRegistration should prevent registration of ExcludeUseCaseInteractor");
    }

    [TestMethod]
    public async Task ExcludeFromRegistration_StreamingRegisterInteractorWithReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(ExcludeFromRegistrationTests));


        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        object? interactor = scope.ServiceProvider.GetService(typeof(ExcludeUseCaseStreamInteractor));

        Assert.IsNull(interactor, "ExcludeFromRegistration should prevent registration of ExcludeUseCaseStreamInteractor");
    }

    [TestMethod]
    public async Task ExcludeFromRegistration_RegisterInteractorWithoutReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();


        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        object? interactor = scope.ServiceProvider.GetService(typeof(ExcludeUseCaseInteractor));

        Assert.IsNull(interactor, "ExcludeFromRegistration should prevent registration of ExcludeUseCaseInteractor");
    }

    [TestMethod]
    public async Task ExcludeFromRegistration_StreamingRegisterInteractorWithoutReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();


        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        object? interactor = scope.ServiceProvider.GetService(typeof(ExcludeUseCaseStreamInteractor));

        Assert.IsNull(interactor, "ExcludeFromRegistration should prevent registration of ExcludeUseCaseStreamInteractor");
    }

    [TestMethod]
    public async Task ExcludeFromRegistration_RegisterEventHandlerWithReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(ExcludeFromRegistrationTests));

        // Remove generic handlers
        serviceCollection.RemoveAll(typeof(IDomainEventHandler<>));

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        object? interactor = scope.ServiceProvider.GetService(typeof(IDomainEventHandler<ExcludeEvent>));

        Assert.IsNull(interactor, "ExcludeFromRegistration should prevent registration of ExcludeUseCaseInteractor");
    }

    [TestMethod]
    public async Task ExcludeFromRegistration_RegisterEventHandlerWithoutReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        // Remove generic handlers
        serviceCollection.RemoveAll(typeof(IDomainEventHandler<>));

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        object? interactor = scope.ServiceProvider.GetService(typeof(IDomainEventHandler<ExcludeEvent>));

        Assert.IsNull(interactor, "ExcludeFromRegistration should prevent registration of ExcludeUseCaseInteractor");
    }
}
