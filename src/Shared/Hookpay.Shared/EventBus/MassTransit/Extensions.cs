
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hookpay.Shared.EventBus.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransitCustom(
        this IServiceCollection services,
        params Assembly[] assemblies    
        )
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(assemblies);       

            x.UsingInMemory((context, cfg) =>
            {             
                cfg.UseConsumeFilter(typeof(ConsumerFilter<>), context);

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
