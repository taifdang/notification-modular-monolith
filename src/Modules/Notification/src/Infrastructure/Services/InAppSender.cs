
using MassTransit;
using MediatR;
using Notification.Application.Notifications.Commands.SendNotification.InApp;
using Notification.Infrastructure.Messages.Contracts;

namespace Notification.Infrastructure.Services;

//public class InAppSender : IConsumer<NotificationDispatched>
//{
//    private readonly IMediator _mediator;
//    public async Task Consume(ConsumeContext<NotificationDispatched> context)
//    {
//        try
//        {
//            await _mediator.Send(new SendNotificationInAppCommand(
//                context.Message.Id, 
//                context.Message.NotificationMessage));
//        }
//        catch
//        {
//            throw;
//        }
//    }
//}
