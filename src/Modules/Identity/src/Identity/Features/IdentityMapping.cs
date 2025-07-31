
using Identity.Identity.Features.RegisteringNewUser;
using Mapster;

namespace Identity.Identity.Features;

public class IdentityMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterNewUserRequestDto, RegisterNewUser>()
            .ConstructUsing(x => 
                new RegisterNewUser(
                    x.FirstName,
                    x.LastName,
                    x.UserName,
                    x.Email,
                    x.Password,
                    x.ConfirmPassword)
                );
    }
}
