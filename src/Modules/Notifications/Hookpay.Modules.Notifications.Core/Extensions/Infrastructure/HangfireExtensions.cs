using Hangfire;
using Hookpay.Shared.EFCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Extensions.Infrastructure;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireCustom(this IServiceCollection services)
    {
        services.AddHangfireStorageMSSQL();
        services.AddHangfireServer(x =>
        {
            x.ServerName = "localhost";
            x.Queues = new[] { "message_queue" }; 
        });

        return services;
    }
}
