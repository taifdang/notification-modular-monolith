
using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using BuildingBlocks.Masstransit;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Common.Interfaces;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Services;

namespace Notification.Infrastructure.Extensions;

public static class InfrasructureExtensions
{
    public static WebApplicationBuilder AddNotificationModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<NotificationEventMapper>();
        builder.Services.AddSingleton<IMasstransitModule, MasstransitExtensions>();
        builder.Services.AddScoped<INotificationDbContext, NotificationDbContext>();
        builder.Services.AddScoped<IRuleBaseService, RuleBaseService>();

        builder.Services.AddCustomDbContext<NotificationDbContext>();
        builder.Services.AddCustomMapster(typeof(NotificationRoot).Assembly);
        builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(NotificationRoot).Assembly));

        builder.Services.AddCustomMediatR();
        builder.Services.AddCustomGrpcClient();

        return builder;
    }
    public static WebApplication UseNotificationModules(this WebApplication app)
    {
        return app;
    }
}
