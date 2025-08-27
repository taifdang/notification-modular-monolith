
using BuildingBlocks.Contracts;
using BuildingBlocks.Signalr;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.ChannelProcessors.Consumers;

public class InappChannelProcessor : IConsumer<NotificationMessage>
{
    private readonly ISignalrHub _signalrHub;
    private readonly ILogger<InappChannelProcessor> _logger;

    public InappChannelProcessor(ISignalrHub signalrHub, ILogger<InappChannelProcessor> logger)
    {
        _signalrHub = signalrHub;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NotificationMessage> context)
    {
        try
        {
            if (context.Message.ChannelType != ChannelType.InApp) 
                return;
    
            //?
            await _signalrHub.ProcessAsync(context.Message.ToString(), context.Message.ChannelType.ToString());

            _logger.LogInformation($"Message is processed");

        }
        catch
        {
            _logger.LogCritical($"occure error");

        }
    }
}
