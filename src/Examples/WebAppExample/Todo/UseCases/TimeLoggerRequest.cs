namespace WebAppExample.Todo.UseCases
{
    public record TimeLoggerRequest(DateTimeOffset Now, long Counter);
}
