
using Hookpay.Modules.Users.Core.Users.Features.RegisterNewUser;
using Hookpay.Modules.Users.Core.UserSetting.Features.CompletingRegisterUser;
using Hookpay.Shared.Core;
using Hookpay.Shared.Core.Events;

namespace Hookpay.Modules.Users.Core;

public class UserEventMapper : IEventMapper
{
    public IIntegrationEvent? MapIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            _ => null
        };
    }

    public IInternalCommand? MapInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            UserCreatedDomainEvent e => new CompleteRegisterUserMono(e.Id)
        };
    }
}
