using BuildingBlocks.Contracts;
using BuildingBlocks.Signalr;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.PersistNotificationProcessor.Sending.SendingInApp;

public class SendInApp : IConsumer<Contracts.NotificationMessage>
{
    private readonly ISignalrHub _signalrHub;
    private readonly ILogger<SendInApp> _logger;

    public SendInApp(ISignalrHub signalrHub, ILogger<SendInApp> logger)
    {
        _signalrHub = signalrHub;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Contracts.NotificationMessage> context)
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
