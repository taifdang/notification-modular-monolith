using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Extensions.Infrastructure;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRCustom(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(NotificationRoot).Assembly));
        return services;
    }
}
