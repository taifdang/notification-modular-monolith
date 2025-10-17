
using BuildingBlocks.Contracts;
using BuildingBlocks.Exception;
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.ValidateNotification;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Validation;

public class ValidationConsumer : IConsumer<NotificationReceived>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    public ValidationConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationReceived> context)
    {
        try
        {
            await _mediator.Send(new ValidateNotificationCommand(
                context.Message.CorrelationId,
                context.Message.NotificationId,
                context.Message.UserId,
                context.Message.Email));
        }
        catch (DomainException ex)
        {
            await _publishEndpoint.Publish(new NotificationFailedEvent(
                context.Message.CorrelationId,
                context.Message.NotificationId,
                "VALIDATE",
                ex.Message));
        }
    }
}
