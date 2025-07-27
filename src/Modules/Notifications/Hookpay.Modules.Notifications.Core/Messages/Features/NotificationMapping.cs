
using Hookpay.Modules.Notifications.Core.Messages.Models;
using Mapster;

namespace Hookpay.Modules.Notifications.Core.Messages.Features;

public class NotificationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MessagePayload, Alert>()
            .Map(x => x.UserId, y => y.EntityId);
    }
}
