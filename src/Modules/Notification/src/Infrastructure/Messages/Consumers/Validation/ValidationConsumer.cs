
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.ValidateNotification;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Messages.Consumers.Validation;

public class ValidationConsumer : IConsumer<NotificationReceived>
{
    private readonly IMediator _mediator;
    public ValidationConsumer(IMediator mediator) => _mediator = mediator;
    public async Task Consume(ConsumeContext<NotificationReceived> context)
    {
        try
        {
            await _mediator.Send(new ValidateNotificationCommand(
                context.Message.NotificationId,
                context.Message.UserId,
                context.Message.Email));
        }
        catch
        {
            throw;
        }
    }
}
