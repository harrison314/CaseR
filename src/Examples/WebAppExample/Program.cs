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
            var builder = WebApplication.CreateSlimBuilder(args);

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            });

            builder.Services.AddCaseR();
            builder.Services.AddCaseRInteractors();

            var app = builder.Build();

            var todosApi = app.MapGroup("/todos");
            todosApi.MapGet("/", async (IUseCase<GetTodoInteractor> getTodoInteractor,
                CancellationToken cancellationToken) =>
            {
                WebAppExample.Todo.UseCases.Todo[] todos = await getTodoInteractor.Execute(new GetTodoInteractorRequest(), cancellationToken);
                return todos;
            });

            app.Run();
        }
    }

    

    [JsonSerializable(typeof(WebAppExample.Todo.UseCases.Todo[]))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {

    }
}
