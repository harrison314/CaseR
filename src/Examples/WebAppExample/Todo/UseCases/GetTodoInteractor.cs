using CaseR;

namespace WebAppExample.Todo.UseCases;

public class GetTodoInteractor : IUseCaseInteractor<GetTodoInteractorRequest, Todo[]>
{
    public GetTodoInteractor()
    {
    }

    public Task<Todo[]> Execute(GetTodoInteractorRequest request, CancellationToken cancellationToken)
    {
        Todo[] sampleTodos = new Todo[]
        {
                new Todo(1, "Walk the dog"),
                new Todo(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
                new Todo(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
                new Todo(4, "Clean the bathroom"),
                new Todo(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
        };

        return Task.FromResult(sampleTodos);
    }
}
