
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Hookpay.Shared.PersistMessageProcessor;

public class PersistMessageProcessorBackgroundService 
    (ILogger<PersistMessageProcessorBackgroundService> logger,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("PersistMessage BackgroundService is started ...");

        await ProcessAsync(stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("PersistMessage BackgroundService is stopped ...");

        return base.StopAsync(cancellationToken);
    }

    private async Task ProcessAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using (var scope = serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPersistMessageProcessor>();

                await service.ProcessAllAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
