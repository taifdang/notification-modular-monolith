using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using MassTransit;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;

public record MessageEvent(int userId, Alert alert);
public class NotificationDispatcher : IConsumer<MessageEvent>
{
    //switch channel
    //email
    //signalr
    public async Task Consume(ConsumeContext<MessageEvent> context)
    {
        Console.WriteLine(context.Message.alert.Body.ToString());
        await Task.Delay(1);
        
    }
}
