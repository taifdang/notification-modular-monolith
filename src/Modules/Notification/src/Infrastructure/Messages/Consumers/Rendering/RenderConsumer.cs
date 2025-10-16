
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.RenderNotification;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Rendering;

public class RenderConsumer : IConsumer<NotificationValidated>
{
    private readonly IMediator _mediator;

    public RenderConsumer(IMediator mediator) => _mediator = mediator;

    public async Task Consume(ConsumeContext<NotificationValidated> context)
    {
        try
        {
            await _mediator.Send(new RenderNotificationCommand(
                context.Message.NotificationId,
                context.Message.CorrelationId,
                context.Message.UserId,
                context.Message.Type,
                context.Message.Priority,
                context.Message.Metadata
            ));
        }
        catch
        {
            throw;
        }
    }
}
