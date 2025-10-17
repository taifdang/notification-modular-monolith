
using BuildingBlocks.Contracts;
using BuildingBlocks.Exception;
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.SendNotification.InApp;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Sending.InApp;

public class InappConsumer : IConsumer<NotificationDispatched>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    public InappConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationDispatched> context)
    {
        try
        {
            await _mediator.Send(new SendNotificationInAppCommand(
                context.Message.NotificationLogId,
                context.Message.NotificationMessage));

            await _publishEndpoint.Publish(new NotificationSentEvent(context.Message.NotificationMessage.CorrelationId));
        }
        catch (DomainException ex)
        {
            await _publishEndpoint.Publish(new NotificationFailedEvent(
                context.Message.NotificationMessage.CorrelationId,
                context.Message.NotificationLogId,
                "SEND_INAPP",
                ex.Message));
        }
    }
}
