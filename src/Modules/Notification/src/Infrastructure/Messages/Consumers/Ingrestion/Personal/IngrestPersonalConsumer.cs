
using BuildingBlocks.Contracts;
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.IngrestNotification.Personal;

namespace Notification.Infrastructure.Messages.Consumers.Ingrestion.Personal;

public class IngrestPersonalConsumer : IConsumer<PersonalNotificationRequested>
{
    private readonly IMediator _mediator;
    public IngrestPersonalConsumer(IMediator mediator) => _mediator = mediator;
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
        catch
        {
            throw;
        }
    }
}
