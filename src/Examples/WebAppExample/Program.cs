using CaseR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using WebAppExample.Todo.UseCases;

namespace WebAppExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            });

            builder.Services.AddCaseR();
            builder.Services.AddCaseRInteractors();

            builder.Services.AddHostedService<TimerLoggingService>();

            WebApplication app = builder.Build();

            RouteGroupBuilder todosApi = app.MapGroup("/todos");
            todosApi.MapGet("/", async (IUseCase<GetTodoInteractor> getTodoInteractor,
                CancellationToken cancellationToken) =>
            {
                WebAppExample.Todo.UseCases.Todo[] todos = await getTodoInteractor.Execute(new GetTodoInteractorRequest(), cancellationToken);
                return todos;
            });

            todosApi.MapGet("/{id}", async (int id, IUseCase<GetTodoItemInteractor> getTodoItemInteractor,
                CancellationToken cancellationToken) =>
            {
                WebAppExample.Todo.UseCases.Todo todo = await getTodoItemInteractor.Execute(id, cancellationToken);
                return todo;
            });

            todosApi.MapGet("/stream", (IUseCase<GetTodoStreamingInteractor> getTodoInteractor,
                CancellationToken cancellationToken) =>
            {
                //TODO: Switch to SSE in .NET 10
                IAsyncEnumerable<WebAppExample.Todo.UseCases.Todo> todos = getTodoInteractor.ExecuteStreaming(new GetTodoInteractorRequest(), cancellationToken);
                return Results.Ok(todos);
            });

            app.Run();
        }
    }



    [JsonSerializable(typeof(WebAppExample.Todo.UseCases.Todo[]))]
    [JsonSerializable(typeof(IAsyncEnumerable<WebAppExample.Todo.UseCases.Todo>))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {

    }
}
