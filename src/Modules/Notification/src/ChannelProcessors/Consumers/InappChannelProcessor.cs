
using BuildingBlocks.Contracts;
using BuildingBlocks.Signalr;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
            var @event = context.Message;

            if (@event.Channel != ChannelType.InApp)
                return;

            
            await _signalrHub.ProcessAsync(@event.Recipient.UserId.ToString(), @event.MessageContent.ToString());

            _logger.LogInformation($"Message is processed");

        }
        catch
        {
            _logger.LogCritical($"occure error");

        }
    }
}
