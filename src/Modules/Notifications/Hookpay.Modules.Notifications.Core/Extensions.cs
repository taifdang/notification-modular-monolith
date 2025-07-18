using Hangfire;
using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
using Hookpay.Modules.Notifications.Core.Messages.Features.FilterMessage;
using Hookpay.Modules.Notifications.Core.Messages.Features.HangfireJobHandler;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.EventBus;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;


namespace Hookpay.Modules.Notifications.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMSSQL<MessageDbContext>();           
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(NotificationRoot).Assembly));
        services.AddMassTransit(x => x.AddConsumers(typeof(NotificationRoot).Assembly));
        services.AddScoped<IBusPublisher, BusPublisher>();
        services.AddScoped<IHangfireJobHandler, HangfireJobHandler>();
        services.AddScoped<MessageAllHandler>();
        services.AddScoped<MessagePersonalHandler>();
        //
        services.AddHangfireStorageMSSQL();
        services.AddHangfireServer(options =>
        {
            options.ServerName = "localhost";
            options.Queues = new[] { "default" };
        });

        services.AddHostedService<FilterMessageWorker>();

        return services;
    }
}
