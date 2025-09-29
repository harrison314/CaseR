using CaseR;
using System.Runtime.CompilerServices;

namespace WebAppExample.Todo.UseCases;

public class GetTodoStreamingInteractor : IUseCaseStreamInteractor<GetTodoInteractorRequest, Todo>
{
    public GetTodoStreamingInteractor()
    {
    }

    public async IAsyncEnumerable<Todo> Execute(GetTodoInteractorRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield return new Todo(1, "Walk the dog");
        await Task.Delay(1000);
        yield return new Todo(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now));
        await Task.Delay(1000);
        yield return new Todo(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        await Task.Delay(1000);
        yield return new Todo(4, "Clean the bathroom");
        await Task.Delay(1000);
        yield return new Todo(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)));
    }
}
