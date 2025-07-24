
using Hookpay.Modules.Users.Core.Users.Features.RegisterNewUser;
using Hookpay.Modules.Users.Core.UserSetting.Features.CompletingRegisterUser;
using Hookpay.Shared.Core;
using Hookpay.Shared.Domain.Events;

namespace Hookpay.Modules.Users.Core;

public class UserEventMapper : IEventMapper
{
    public IIntegrationEvent? MapIntegrationEvent(IDomainEvent @event)
    {
        throw new NotImplementedException();
    }

    public IInternalCommand? MapInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            UserCreatedDomainEvent e => new CompleteUserRegisterUserMono(e.Id)
        };
    }
}
