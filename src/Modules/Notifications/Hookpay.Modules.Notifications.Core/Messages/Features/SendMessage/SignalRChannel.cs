
using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public class SignalRChannel : INotificationChannel
{
    public PushType PushType => PushType.InApp;

    public Task SendAsync(Alert alert)
    {
        Console.WriteLine($" Handler {alert.Title} with body = {alert.Body}");
        return Task.CompletedTask;

    }
}
