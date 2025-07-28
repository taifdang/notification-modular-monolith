using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Background;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
using Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.EventBus;
using Hookpay.Shared.Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Modules.Notifications.Core.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddNotificationModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMSSQL<MessageDbContext>();
        builder.Services.AddMediatRCustom();
        //builder.Services.AddMassTransit(x => x.AddConsumers(typeof(NotificationRoot).Assembly));
        builder.Services.AddMapsterCustom(typeof(NotificationRoot).Assembly);
        builder.Services.AddGrpcClientCustom();
        builder.Services.AddHangfireCustom();

        builder.Services.AddScoped<IBusPublisher, BusPublisher>();
        builder.Services.AddScoped<IMessageEventInternalProcessor, MessageEventInternalProcessor>();
        builder.Services.AddScoped<ICreateMessageProcessor, CreateMesssageProcessor>();

        builder.Services.AddScoped<INotificationChannel, EmailChannel>();
        builder.Services.AddScoped<INotificationChannel, SignalrChannel>();

        builder.Services.AddHostedService<MessageEventInternalBackgroundService>();

        return builder;
    }

    public static WebApplication UseNotificationModules(this WebApplication app)
    {
        return app;
    }
}
