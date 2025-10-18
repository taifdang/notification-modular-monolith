using MassTransit;

namespace BuildingBlocks.Masstransit;

public interface IStateMachineModule
{
    void ConfigureStateMachines(IBusRegistrationConfigurator cfg);
    //void ConfigureStateMachines(IRegistrationContext cfg);
}
