using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Signalr;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Logging;
using Notification.Data;
using Notification.PersistNotificationProcessor.Contracts;

namespace Notification.PersistNotificationProcessor.Sending.SendingInApp;

public class InAppSender : IConsumer<NotificationDispatched>
{
    private readonly ISignalrHub _signalrHub;
    private readonly ILogger<InAppSender> _logger;
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public InAppSender(ISignalrHub signalrHub, ILogger<InAppSender> logger, NotificationDbContext notificationDbContext, IPublishEndpoint publishEndpoint)
    {
        _signalrHub = signalrHub;
        _logger = logger;
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationDispatched> context)
    {
        Guard.Against.Null(context.Message, nameof(NotificationDispatched));

        _logger.LogInformation($"consumer for {nameof(NotificationDispatched)} is started");

        var @event = context.Message;

        // topup flow
        context.Headers.TryGetHeader("correlationId", out var correlationId);

        //transport: inmemory = skip, filter at consumer
        if (@event.Channel != ChannelType.InApp)
        {
            return;
        }

        var notificationLog = await _notificationDbContext.NotificationLogs.FindAsync(context.Message.RequestId);

        if (notificationLog == null)
        {
            _logger.LogWarning($"consumer for {nameof(NotificationDispatched)} not found logId {context.Message.RequestId}");
            return;
        }

        try
        {  
            await _signalrHub.ProcessAsync(@event.Recipient.UserId.ToString(), @event.Message.ToString());

            notificationLog.UpdateStatus(NotificationLogs.Enums.Status.Sent);

            await _notificationDbContext.SaveChangesAsync();

            await _publishEndpoint.Publish(
                new NotificationSentEvent(Guid.Parse(correlationId!.ToString()!)));
        }
        catch(Exception error)
        {
            _logger.LogError(error, "Failed to send InApp notification for MessageId {MessageId}", @event.RequestId);

            notificationLog.UpdateStatus(NotificationLogs.Enums.Status.Failed, error.ToString());

            await _notificationDbContext.SaveChangesAsync();

            await _publishEndpoint.Publish(
               new TopupFailedEvent(Guid.Parse(correlationId!.ToString()!),"Internal error"));
        }
    }
}
