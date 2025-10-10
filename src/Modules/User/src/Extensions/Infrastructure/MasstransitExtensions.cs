using BuildingBlocks.Masstransit;
using MassTransit;
using User.Identity.Consumers.RegisteringNewUser;

namespace User.Extensions.Infrastructure;

public class MasstransitExtensions : IMasstransitModule
{
    public void ConfigureTopology(IBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.ReceiveEndpoint("identity-user", e =>
        {
            e.ConfigureConsumer<RegisterNewUserHandler>(context);
        });
    }
}
