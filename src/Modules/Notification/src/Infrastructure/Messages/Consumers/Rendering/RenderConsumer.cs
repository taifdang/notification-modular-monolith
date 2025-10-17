
using BuildingBlocks.Contracts;
using BuildingBlocks.Exception;
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.RenderNotification;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Rendering;

public class RenderConsumer : IConsumer<NotificationValidated>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public RenderConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationValidated> context)
    {
        try
        {
            await _mediator.Send(new RenderNotificationCommand(
                context.Message.CorrelationId,
                context.Message.NotificationId,
                context.Message.UserId,
                context.Message.Type,
                context.Message.Priority,
                context.Message.Payload
            ));
        }
        catch (DomainException ex)
        {
            await _publishEndpoint.Publish(new NotificationFailedEvent(
                context.Message.CorrelationId,
                context.Message.NotificationId,
                "RENDER",
                ex.Message));
        }
    }
}
