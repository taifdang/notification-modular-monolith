using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Features.FilterMessage;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.SignalR;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMSSQL<MessageDbContext>();      
        //.AddHostedService<FilterMessageWorker>();      
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(NotificationRoot).Assembly));
        services.AddMassTransit(x => x.AddConsumers(typeof(NotificationRoot).Assembly));
        return services;
    }
}
