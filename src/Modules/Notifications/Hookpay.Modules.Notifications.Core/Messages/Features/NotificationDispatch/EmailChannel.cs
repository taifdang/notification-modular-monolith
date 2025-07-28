using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;

public class EmailChannel : INotificationChannel
{
    public PushType PushType => PushType.Email;

    public Task SendAsync(int target,Alert alert)
    {
        throw new NotImplementedException();
    }
}
