using CaseR;

namespace WebAppExample.Todo.UseCases
{

    public class TimeLoggerInteractor : IUseCaseInteractor<TimeLoggerRequest, Unit>
    {
        private readonly ILogger<TimeLoggerInteractor> logger;

        public TimeLoggerInteractor(ILogger<TimeLoggerInteractor> logger)
        {
            this.logger = logger;
        }

        public Task<Unit> Execute(TimeLoggerRequest request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("TimeLogger: {Now} - Counter: {Counter}", request.Now, request.Counter);

            return Unit.Task;
        }
    }
}
