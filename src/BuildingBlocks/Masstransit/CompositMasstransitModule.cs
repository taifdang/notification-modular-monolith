using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Masstransit;

public static class CompositMasstransitModule
{
    public static void ApplyModuleTopology(this IBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        var modules = context.GetRequiredService<IEnumerable<IMasstransitModule>>();

        foreach (var module in modules)
        {
            module.ConfigureTopology(cfg, context);
        }
    }
}
