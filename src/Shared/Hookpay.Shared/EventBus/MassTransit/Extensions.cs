
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
            //ref: https://stackoverflow.com/questions/17326185/what-are-the-different-kinds-of-cases
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumers(assemblies);       

            x.UsingInMemory((context, cfg) =>
            {     
                //middleware for write event inbox
                //cfg.UseConsumeFilter(typeof(ConsumerFilter<>), context);

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
