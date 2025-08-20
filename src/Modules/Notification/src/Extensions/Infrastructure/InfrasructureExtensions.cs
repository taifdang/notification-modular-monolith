using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Notification.Data;

namespace Notification.Extensions.Infrastructure;
public static class InfrasructureExtensions
{
    public static WebApplicationBuilder AddNotificationModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<NotificationEventMapper>();
        builder.Services.AddMediatRCustom();
        builder.Services.AddMapsterCustom(typeof(NotificationRoot).Assembly);
        builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(NotificationRoot).Assembly));

        builder.Services.AddMssql<NotificationDbContext>();

        builder.Services.AddGrpcClientCustom();

        return builder;
    }

    public static WebApplication UseNotificationModules(this WebApplication app)
    {
        return app;
    }
}
