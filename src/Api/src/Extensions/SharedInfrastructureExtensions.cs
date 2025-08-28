using BuildingBlocks.Core;
using BuildingBlocks.Hangfire;
using BuildingBlocks.Jwt;
using BuildingBlocks.Masstransit;
using BuildingBlocks.OpenApi;
using BuildingBlocks.PersistMessageProcessor;
using BuildingBlocks.Signalr;
using BuildingBlocks.Web;
using FluentValidation.AspNetCore;
using Hangfire;
using Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Notification;
using UserProfile;

namespace Api.Extensions;

public static class SharedInfrastructureExtensions
{
    public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        //builder.Services.AddJwt();
        builder.Services.AddJwtAuth();

        //persistMessage
        builder.Services.AddPersistMessageProcessor();

        builder.Services.AddControllers();

        //builder.Services.AddAuthentication("Identity.Application").AddCookie();

        //validation
        builder.Services.AddFluentValidation();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSwaggerCustom();

        builder.Services.AddSingleton<ISignalrHub, SignalrHub>();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

        builder.Services.AddHangfireStorageMssql();

        builder.Services.AddCustomMasstransit(
            TransportType.InMemory,
            AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddMemoryCache();

        builder.Services.AddScoped<IEventMapper, IdentityEventMapper>();
        builder.Services.AddScoped<IEventMapper, UserProfileEventMapper>();
        builder.Services.AddScoped<IEventMapper, NotificationEventMapper>();


        return builder;
    }

    public static WebApplication UseSharedInfrastructure(this WebApplication app)
    {
        app.MapHub<SignalrHub>("/hubs");

        app.MapHangfireDashboard("/hangfire");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseCorrelationId();

        app.MapControllers();

        

        return app;
    }
}
