using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
using Hookpay.Modules.Notifications.Core.Messages.Features.HangfireJobHandler;
using Hookpay.Shared.EFCore;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;


namespace Hookpay.Modules.Notifications.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMSSQL<MessageDbContext>();      
        //.AddHostedService<FilterMessageWorker>();      
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(NotificationRoot).Assembly));
        services.AddMassTransit(x => x.AddConsumers(typeof(NotificationRoot).Assembly));
        services.AddSingleton<IHangfireJobHandler, HangfireJobHandler>();
        services.AddSingleton<MessageAllHandler>();
        services.AddScoped<MessagePersonalHandler>();

        return services;
    }
}
