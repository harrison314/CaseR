using CaseR;

namespace WebAppExample.Todo.UseCases;

public class GetTodoItemInteractor : IUseCaseInteractor<int, Todo>
{
    private readonly IDomainEventPublisher domainEventPublisher;

    public GetTodoItemInteractor(IDomainEventPublisher domainEventPublisher)
    {
        this.domainEventPublisher = domainEventPublisher;
    }

    public async ValueTask<Todo> Execute(int request, CancellationToken cancellationToken)
    {
        await this.domainEventPublisher.Publish(new GetTodoItemInteractorEvent(request), cancellationToken);
        return new Todo(request, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
    }
}

public record GetTodoItemInteractorEvent(int Id) : IDomainEvent;

public class GetTodoItemInteractorEventHandler : IDomainEventHandler<GetTodoItemInteractorEvent>
{
    public ValueTask Handle(GetTodoItemInteractorEvent domainEvent, CancellationToken cancellationToken)
    {
        //...
        return ValueTask.CompletedTask;
    }
}