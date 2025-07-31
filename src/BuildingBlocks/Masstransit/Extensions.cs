
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Masstransit;

public static class Extensions
{
    //ref: https://masstransit.io/documentation/configuration/middleware/scoped
    public static IServiceCollection AddMasstransitCustom(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddMassTransit(x =>
        {
            //ref: https://stackoverflow.com/questions/17326185/what-are-the-different-kinds-of-cases
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumers(assemblies);

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
