

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.PersistMessageProcessor;

public class PersistMessageBackgroundService(
    ILogger<PersistMessageBackgroundService> logger,
    IServiceProvider serviceProvider ) 
    : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Persist message background service started");
        await ProcessAsync(stoppingToken);
    }

    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Persist message background service stopped");
        return base.StopAsync(cancellationToken);
    }

    private async Task ProcessAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await using (var scope = serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPersistMessageProcessor>();
                await service.ProcessAllAsync(cancellationToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }
}
