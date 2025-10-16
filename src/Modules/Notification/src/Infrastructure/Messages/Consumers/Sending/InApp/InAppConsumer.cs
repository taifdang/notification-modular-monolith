
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.SendNotification.InApp;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Sending.InApp;

public class InappConsumer : IConsumer<NotificationDispatched>
{
    private readonly IMediator _mediator;
    public InappConsumer(IMediator mediator) => _mediator = mediator;

    public async Task Consume(ConsumeContext<NotificationDispatched> context)
    {
        try
        {
            await _mediator.Send(new SendNotificationInAppCommand(
                context.Message.NotificationLogId,
                context.Message.NotificationMessage));
        }
        catch
        {
            throw;
        }
    }
}
