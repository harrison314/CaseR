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

    [TestMethod]
    public async Task AddCaseR_RegisterWithAutoscope_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);

        IAutoScopedUseCase<PingPongInteractor> interactor = sp.GetRequiredService<IAutoScopedUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute<PingPongInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);
    }

    [TestMethod]
    public async Task AddCaseR_RegisterWithAutoscopeInScope_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IAutoScopedUseCase<PingPongInteractor> interactor = scope.ServiceProvider.GetRequiredService<IAutoScopedUseCase<PingPongInteractor>>();

        Pong pong = await interactor.Execute(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);
    }

    [TestMethod]
    public async Task AddCaseR_RegisterStreamingAndUseWithReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongStreamingInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongStreamingInteractor>>();

        IAsyncEnumerable<Pong> pong = interactor.ExecuteStreaming<PingPongStreamingInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);

        await foreach (Pong p in pong)
        {
            Assert.IsNotNull(p);
        }
    }

    [TestMethod]
    public async Task AddCaseR_RegisterStreamingAndUseWithGenerator_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongStreamingInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongStreamingInteractor>>();

        IAsyncEnumerable<Pong> pong = interactor.ExecuteStreaming<PingPongStreamingInteractor, Ping, Pong>(new Ping(), CancellationToken.None);

        Assert.IsNotNull(pong);
        await foreach (Pong p in pong)
        {
            Assert.IsNotNull(p);
        }
    }

    [TestMethod]
    public async Task AddCaseR_RegisterCategoryAndUseWithReflection_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests), "Api");

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<ApiCategoryInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<ApiCategoryInteractor>>();

        ApiResponse response = await interactor.Execute<ApiCategoryInteractor, ApiRequest, ApiResponse>(new ApiRequest(), CancellationToken.None);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task AddCaseR_RegisterMultipleCategoryOnSingleInteractor_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests), "Api");

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<MultiCategoryInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<MultiCategoryInteractor>>();

        MultiCategoryResponse response = await interactor.Execute<MultiCategoryInteractor, MultiCategoryRequest, MultiCategoryResponse>(new MultiCategoryRequest(), CancellationToken.None);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task AddCaseR_RegisterDifferentCategoryExclusion_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests), "BackgroundJob");

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<BackgroundJobInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<BackgroundJobInteractor>>();

        BackgroundJobResponse response = await interactor.Execute<BackgroundJobInteractor, BackgroundJobRequest, BackgroundJobResponse>(new BackgroundJobRequest(), CancellationToken.None);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task AddCaseR_CategoryInteractorExcludedFromDefaultRegistration_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests));

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<PingPongInteractor> pingPongInteractor = scope.ServiceProvider.GetRequiredService<IUseCase<PingPongInteractor>>();
        Assert.IsNotNull(pingPongInteractor);

        ApiCategoryInteractor? categoryInteractors = sp.GetService<ApiCategoryInteractor>();
        Assert.IsNull(categoryInteractors);
    }

    [TestMethod]
    public void RegistrationCathegory_ReflectionPropertiesAndMultipleAttributes_Success()
    {
        Type apiInteractorType = typeof(ApiCategoryInteractor);
        object[] apiAttributes = apiInteractorType.GetCustomAttributes(typeof(RegistrationCathegoryAttribute), false);

        Assert.HasCount(1, apiAttributes);
        RegistrationCathegoryAttribute apiAttribute = (RegistrationCathegoryAttribute)apiAttributes[0];
        Assert.AreEqual("Api", apiAttribute.CathegoryName);

        Type multiCategoryType = typeof(MultiCategoryInteractor);
        object[] multiAttributes = multiCategoryType.GetCustomAttributes(typeof(RegistrationCathegoryAttribute), false);

        Assert.HasCount(2, multiAttributes);
        HashSet<string> categoriesFound = new HashSet<string>();
        foreach (RegistrationCathegoryAttribute attr in multiAttributes)
        {
            categoriesFound.Add(attr.CathegoryName);
        }

        Assert.Contains("Api", categoriesFound);
        Assert.Contains("BackgroundJob", categoriesFound);
    }

    [TestMethod]
    public async Task AddCaseR_RegisterCategoryAndUseWithGenerator_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors();

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);
        await using AsyncServiceScope scope = sp.CreateAsyncScope();

        IUseCase<ApiCategoryInteractor> interactor = scope.ServiceProvider.GetRequiredService<IUseCase<ApiCategoryInteractor>>();

        ApiResponse response = await interactor.Execute<ApiCategoryInteractor, ApiRequest, ApiResponse>(new ApiRequest(), CancellationToken.None);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task AddCaseR_RegisterCategoryAutoscope_Success()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests), "Api");

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);

        IAutoScopedUseCase<ApiCategoryInteractor> interactor = sp.GetRequiredService<IAutoScopedUseCase<ApiCategoryInteractor>>();

        ApiResponse response = await interactor.Execute<ApiCategoryInteractor, ApiRequest, ApiResponse>(new ApiRequest(), CancellationToken.None);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void AddCaseR_NonExistentCategoryRegistration_NoInteractorsRegistered()
    {
        ServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCaseR();
        serviceCollection.AddCaseRInteractors(typeof(RegistrationTests), "NonExistentCategory");

        ServiceProvider sp = serviceCollection.BuildServiceProvider(true);

        ApiCategoryInteractor? apiInteractor = sp.GetService<ApiCategoryInteractor>();
        Assert.IsNull(apiInteractor);
    }
}
