
using BuildingBlocks.Contracts;
using BuildingBlocks.Exception;
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.DispathNotification;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Dispatching;

public class DispatchConsumer : IConsumer<NotificationRendered>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    public DispatchConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationRendered> context)
    {
        try
        {
            await _mediator.Send(new DispatchNotificationCommand(context.Message.NotificationMessage));
        }
        catch (DomainException ex)
        {
            await _publishEndpoint.Publish(new NotificationFailedEvent(
                context.Message.NotificationMessage.CorrelationId,
                context.Message.NotificationMessage.NotificationId,
                "DISPATCH",
                ex.Message));
        }
    }
}
