using Hookpay.Modules.Users.Core.Users.Dtos;
using Mapster;

namespace Hookpay.Modules.Users.Core.Users.Features;

public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Models.Users, UserDto>();
    }
}
