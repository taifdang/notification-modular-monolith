using BuildingBlocks.Masstransit;
using MassTransit;
using Wallet.Identity.Consumers.RegisteringNewUser;

namespace Wallet.Extensions.Infrastructure;

public class MasstransitExtensions : IMasstransitModule
{
    public void ConfigureTopology(IBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.ReceiveEndpoint("identity-wallet", e =>
        {          
            e.ConfigureConsumer<RegisterNewUserHandler>(context);
        });

    }
}
