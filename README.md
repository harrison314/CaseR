# CaseR
CaseR is not another MediatR clone, but tries to solve the same problem in a different way (in context .NET 10).

The main task of this library is to model use cases in an application and separate cross-cutting concerns (like logging, caching, monitoring, transactions,...)
and support vertical slice architecture
in ASP\.NET Core 8+ applications, even with support for AOT compilation.


> [!NOTE]
> CaseR is a conceptual project.  
> The following criticism does not only apply to the MediatR library, but also to its clones and some similar libraries. In my opinion, MediatR showed a good direction, which was due to the time of its creation. The CaseR project tries to show a different principle but solve the same problems.

I tried to solve mainly these problems:
* Interface `IMediator` is too generic, it's like injecting an `IServiceProvider`,
  so class dependencies or MinimalAPI don't make it clear what the class requires.
* In a project using a mediator, it is difficult to navigate because it is not easy to get to the handler implementation.
* MediatR is not type-safe in compile time. It is possible to call `IMediator.Send()` with a request for which there is no handler.
* The necessity to use `IRequest<>` and `IResponse`, we understand why these interfaces are in MediatR, but it bothers me a bit.

After a few projects where I used MediatR I realized a few things.
Developers actually use MediatR to implement their use cases.
There is no CQRS support, this arises naturally by having each HTTP request implemented in a separate class.
It also doesn't directly implement the message queue either.

Therefore, I decided to create a library that uses the correct terminology for _Use Case_ (and _interactor_ from Clean Architecture).

## Key Features
* Modeling use cases
* Built entirely on top of Dependency Injection
* Zero runtime reflection after registration
  * _CaseR.SourceGenerator_ eliminate reflection at registration
* Direct code reference to business logic (no problem with trimming and orientation in codebase - F12 work)
* Compile-time type safety (using generic constraints and source generator)
* Interaction interceptor pipeline
* Supports keyed pipelines
* Supports domain events publisher

## Get started

### 1. Install nugets
Install nugets into project using:

`dotnet add package CaseR`

`dotnet add package CaseR.SourceGenerator`

### 2. Register CaseR services
```cs
builder.Services.AddCaseR();
builder.Services.AddCaseRInteractors();
```
### 3. Create interactor

```cs
public record GetTodoInteractorRequest();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

public class GetTodoInteractor : IUseCaseInterceptor<GetTodoInteractorRequest, Todo[]>
{
    public GetTodoInteractor()
    {
        
    }

    public ValueTask<Todo[]> Execute(GetTodoInteractorRequest request, CancellationToken cancellationToken)
    {
        Todo[] sampleTodos = new Todo[] 
        {
                new Todo(1, "Walk the dog"),
                new Todo(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
                new Todo(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
                new Todo(4, "Clean the bathroom"),
                new Todo(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
        };

        return new ValueTask<Todo[]>(sampleTodos);
    }
}
```

### 4. Use use case in minimal API
```cs
var todosApi = app.MapGroup("/todos");
            todosApi.MapGet("/", async (IUseCase<GetTodoInteractor> getTodoInteractor,
                CancellationToken cancellationToken) =>
            {
                WebAppExample.Todo.UseCases.Todo[] todos = await getTodoInteractor.Execute(new GetTodoInteractorRequest(), cancellationToken);
                return todos;
            });
```

## Domain events

### 1. Create domain event
```cs
public record GetTodoItemInteractorEvent(int Id) : IDomainEvent;
```

### 2. Create domain event handlers
```cs
public class GetTodoItemInteractorEventHandler : IDomainEventHandler<GetTodoItemInteractorEvent>
{
    public ValueTask Handle(GetTodoItemInteractorEvent domainEvent, CancellationToken cancellationToken)
    {
        //...
        return ValueTask.CompletedTask;
    }
}
```

### 3. Publish event
Inject `IDomainEventPublisher` into your interactor or handler and use it to publish events.
```cs
IDomainEventPublisher domainEventPublisher
```

Publish event:
```cs
 await this.domainEventPublisher.Publish(new GetTodoItemInteractorEvent(request), cancellationToken);
```

### Use Case interceptors
Interceptors are used to create a pipeline that wraps the interactor call.

```cs
public class ElapsedTimeInterceptor<TRequest, TResponse> : IUseCaseInterceptor<TRequest, TResponse>
{
    private readonly ILogger<ElapsedTimeInterceptor<TRequest, TResponse>> logger;

    public ElapsedTimeInterceptor(ILogger<ElapsedTimeInterceptor<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<TResponse> InterceptExecution(IUseCaseInteractor<TRequest, TResponse> useCaseInteractor, TRequest request, UseCasePerformDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        long timestamp = Stopwatch.GetTimestamp();
        try
        {
            return await next(request).ConfigureAwait(false);
        }
        finally
        {
            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(timestamp);
            this.logger.LogCritical("Elapsed {ElapsedTime}ms in {InteractorName}.", elapsedTime.TotalMilliseconds, useCaseInteractor.GetType().Name);
        }
    }
}
```

And register interceptors in registration:
```cs
builder.Services.AddCaseR(options =>
  {
    options.AddGenericInterceptor(typeof(ElapsedTimeInterceptor<,>));
    // register another interceptors
  });
builder.Services.AddCaseRInteractors();
```

Or use keyed piplines:
```cs
builder.Services.AddKeyedCaseR("GrpcPipeline", options =>
  {
    options.AddGenericInterceptor(typeof(AnotherInterceptor<,>));
    options.AddGenericInterceptor(typeof(NestedLogInterceptor<,>));
  });

builder.Services.AddKeyedCaseR("MinimalApiPipeline", options =>
  {
    options.AddGenericInterceptor(typeof(AnotherInterceptor<,>));
    options.AddGenericInterceptor(typeof(NestedLogInterceptor<,>));
  });
```

Usage:
```cs
var todosApi = app.MapGroup("/todos");
            todosApi.MapGet("/", async ([FromKeyedServices("MinimalApiPipeline")] IUseCase<GetTodoInteractor> getTodoInteractor,
                CancellationToken cancellationToken) =>
            {
                WebAppExample.Todo.UseCases.Todo[] todos = await getTodoInteractor.Execute(new GetTodoInteractorRequest(), cancellationToken);
                return todos;
            });
```

## CaseR 
[![NuGet Status](http://img.shields.io/nuget/v/CaseR.svg?style=flat)](https://www.nuget.org/packages/CaseR/)
The CaseR library contains all the logic and can be used without a source generator.

However, it has two limitations:
* Registering interactors is done using reflection `builder.Services.AddCaseRInteractors(typeof(Program));`,
* In the execute method, generic parameters must be passed when calling `await useCase.Execute<TInteractor, TRequest, TResponse>(request, cancellationToken);`.

## CaseR.SourceGenerator 
[![NuGet Status](http://img.shields.io/nuget/v/CaseR.SourceGenerator.svg?style=flat)](https://www.nuget.org/packages/CaseR.SourceGenerator/)

The CaseR.SourceGenerator library is a source generator that generates the necessary code for the CaseR library to work without reflection
and typed `Execute` method.

