using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Signalr;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Data;

namespace Notification.PersistNotificationProcessor.Sending.SendingInApp;

public class InAppSender : IConsumer<Contracts.NotificationMessage>
{
    private readonly ISignalrHub _signalrHub;
    private readonly ILogger<InAppSender> _logger;
    private readonly NotificationDbContext _notificationDbContext;

    public InAppSender(ISignalrHub signalrHub, ILogger<InAppSender> logger, NotificationDbContext notificationDbContext)
    {
        _signalrHub = signalrHub;
        _logger = logger;
        _notificationDbContext = notificationDbContext;
    }

    public async Task Consume(ConsumeContext<Contracts.NotificationMessage> context)
    {
        Guard.Against.Null(context.Message, nameof(Contracts.NotificationMessage));

        var @event = context.Message;

        //transport: inmemory = skip, filter at consumer
        if (@event.Channel != ChannelType.InApp)
        {
            return;
        }

        var notificationLog = await _notificationDbContext.NotificationLogs.FindAsync(context.Message.MessageId);

        if (notificationLog == null)
        {
            return;
        }

        try
        {        
            await _signalrHub.ProcessAsync(@event.Recipient.UserId.ToString(), @event.Message.ToString());

            notificationLog.UpdateStatus(NotificationLogs.Enums.Status.Sent);

            await _notificationDbContext.SaveChangesAsync();
        }
        catch(Exception error)
        {
            _logger.LogError(error, "Failed to send InApp notification for MessageId {MessageId}", @event.MessageId);

            notificationLog.UpdateStatus(NotificationLogs.Enums.Status.Failed, error.ToString());

            await _notificationDbContext.SaveChangesAsync();
        }
    }
}
