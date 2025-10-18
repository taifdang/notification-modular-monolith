using FluentAssertions.Common;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Masstransit;

public static class CompositStateMachine
{
    public static void ApplyModuleStateMachines(this IBusRegistrationConfigurator cfg, IServiceCollection service)
    {
        using var provider = service.BuildServiceProvider();
        var modules = provider.GetRequiredService<IEnumerable<IStateMachineModule>>();

        foreach (var module in modules)
        {
            module.ConfigureStateMachines(cfg);
        }
    }
}
