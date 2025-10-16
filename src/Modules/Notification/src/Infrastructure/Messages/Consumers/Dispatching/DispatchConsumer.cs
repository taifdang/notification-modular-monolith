
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.DispathNotification;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Dispatching;

public class DispatchConsumer : IConsumer<NotificationRendered>
{
    private readonly IMediator _mediator;
    public DispatchConsumer(IMediator mediator) => _mediator = mediator;

    public async Task Consume(ConsumeContext<NotificationRendered> context)
    {
        try
        {
            await _mediator.Send(new DispatchNotificationCommand(
                context.Message.NotificationId,
                context.Message.NotificationMessage));
        }
        catch
        {
            throw;
        }
    }
}
