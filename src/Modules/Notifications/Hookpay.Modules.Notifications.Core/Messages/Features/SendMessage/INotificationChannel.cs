
using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public interface INotificationChannel
{
    PushType PushType {  get; }
    Task SendAsync(Alert alert);
}
