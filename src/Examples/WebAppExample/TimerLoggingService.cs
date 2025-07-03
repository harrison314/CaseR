

using CaseR;
using WebAppExample.Todo.UseCases;

namespace WebAppExample;

public class TimerLoggingService : BackgroundService
{
    private readonly IAutoScopedUseCase<TimeLoggerInteractor> timeLoggerInteractor;

    public TimerLoggingService(IAutoScopedUseCase<TimeLoggerInteractor> timeLoggerInteractor)
    {
        this.timeLoggerInteractor = timeLoggerInteractor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        long counter = 0;
        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(15));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await this.timeLoggerInteractor.Execute(new TimeLoggerRequest(DateTimeOffset.UtcNow, counter), stoppingToken);
        
            counter++;
        }
    }
}
