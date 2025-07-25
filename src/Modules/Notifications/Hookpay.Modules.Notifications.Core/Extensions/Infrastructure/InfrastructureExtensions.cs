﻿using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Background;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.EventBus;
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
        builder.Services.AddGrpcClientCustom();
        builder.Services.AddHangfireCustom();

        builder.Services.AddScoped<IBusPublisher, BusPublisher>();
        builder.Services.AddScoped<IPersistMessageInternalProcessor, PersistMessageInternalProcessor>();
        builder.Services.AddScoped<ICreateMessageProcessor, CreateMesssageProcessor>();

        builder.Services.AddHostedService<PersistMesssageInternalBackgroundService>();

        return builder;
    }

    public static WebApplication UseNotificationModules(this WebApplication app)
    {
        return app;
    }
}
