using MassTransit;

namespace BuildingBlocks.Masstransit;

public interface IMasstransitModule
{
    void ConfigureTopology(IBusFactoryConfigurator cfg, IRegistrationContext context);
}
