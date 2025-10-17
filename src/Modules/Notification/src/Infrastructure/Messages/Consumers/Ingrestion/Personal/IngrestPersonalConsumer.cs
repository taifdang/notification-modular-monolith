
using BuildingBlocks.Contracts;
using BuildingBlocks.Exception;
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.IngrestNotification.Personal;

namespace Notification.Infrastructure.Messages.Consumers.Ingrestion.Personal;

public class IngrestPersonalConsumer : IConsumer<PersonalNotificationRequested>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    public IngrestPersonalConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<PersonalNotificationRequested> context)
    {
        try
        {
            await _mediator.Send(new IngresNotificationPersonalCommand(
                context.Message.CorrelationId,
                context.Message.NotificationType,
                context.Message.Recipient,
                context.Message.Payload,
                context.Message.Priority));
        }
        catch(DomainException ex)
        {
            await _publishEndpoint.Publish(new NotificationFailedEvent(
                context.Message.CorrelationId,
                null,
                "RENDER",
                ex.Message));
        }
    }
}
