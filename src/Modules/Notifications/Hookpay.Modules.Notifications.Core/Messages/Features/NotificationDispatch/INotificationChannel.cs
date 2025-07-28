using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;

public interface INotificationChannel
{
    PushType PushType {  get; }
    Task SendAsync(int target,Alert alert);
}
