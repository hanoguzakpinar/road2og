namespace WorkerExample;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(3000, stoppingToken);
        }
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Servis başlatılıyor.");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Servis durduruluyor.");
        return base.StopAsync(cancellationToken);
    }
}
