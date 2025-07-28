
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hookpay.Shared.EventBus.MassTransit;

public static class Extensions
{
    //ref: https://masstransit.io/documentation/configuration/middleware/scoped
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

                cfg.UseMessageRetry(AddRetryConfiguration);
            });

            
        });

        return services;
    }

    private static void AddRetryConfiguration(IRetryConfigurator retryConfigurator)
    {
        //ref: https://markgossa.com/2022/06/masstransit-exponential-back-off.html
        //ref: https://masstransit.io/documentation/concepts/exceptions
        retryConfigurator
            .Exponential(
                3,
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMinutes(120),
                TimeSpan.FromMilliseconds(200))
            .Ignore<ValidationException>();//elseif
    }
}
