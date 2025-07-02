using CaseR;

namespace WebAppExample.Todo.UseCases;

public class GetTodoItemInteractorEventHandler : IDomainEventHandler<GetTodoItemInteractorEvent>
{
    public Task Handle(GetTodoItemInteractorEvent domainEvent, CancellationToken cancellationToken)
    {
        //...
        return Task.CompletedTask;
    }
}