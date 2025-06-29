using CaseR;

namespace WebAppExample.Todo.UseCases;

public class GetTodoItemInteractorEventHandler : IDomainEventHandler<GetTodoItemInteractorEvent>
{
    public ValueTask Handle(GetTodoItemInteractorEvent domainEvent, CancellationToken cancellationToken)
    {
        //...
        return ValueTask.CompletedTask;
    }
}