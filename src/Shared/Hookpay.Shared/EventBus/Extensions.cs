using Hookpay.Shared.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.EventBus;

public static class Extensions
{
    public static IServiceCollection AddMassTransitCustom(this IServiceCollection services)
    {
        return services.AddMassTransit(x =>
        {          
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
