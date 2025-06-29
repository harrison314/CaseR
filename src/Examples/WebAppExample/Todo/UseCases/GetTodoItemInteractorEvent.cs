using CaseR;

namespace WebAppExample.Todo.UseCases;

public record GetTodoItemInteractorEvent(int Id) : IDomainEvent;
