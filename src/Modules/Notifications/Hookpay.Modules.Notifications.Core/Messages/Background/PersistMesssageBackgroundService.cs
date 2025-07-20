using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hookpay.Modules.Notifications.Core.Messages.Background;

public class PersistMesssageBackgroundService(ILogger<PersistMesssageBackgroundService> logger, IServiceProvider provider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Persistant filter messsage background service start");

        await ProcessAsync(stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Persistant filter messsage background service stop");

        return base.StopAsync(cancellationToken);
    }

    private async Task ProcessAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await using (var scope = provider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPersistMessageProcessor>();
                
                await service.ProcessAllAsync(cancellationToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }

}
