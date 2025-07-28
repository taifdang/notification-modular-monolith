
using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public class EmailChannel : INotificationChannel
{
    public PushType PushType => PushType.Email;

    public Task SendAsync(Alert alert)
    {
        throw new NotImplementedException();
    }
}
