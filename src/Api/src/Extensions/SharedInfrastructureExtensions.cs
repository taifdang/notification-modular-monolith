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
using Notification;
using System.Reflection;
using User;
using Wallet;

namespace Api.Extensions;

//ref: https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-9.0&tabs=dotnet
public static class SharedInfrastructureExtensions
{
    public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
    {
        Assembly[] assemblies = new Assembly[]
        {
            typeof(IdentityRoot).Assembly,
            typeof(WalletRoot).Assembly,
            typeof(UserRoot).Assembly,
            typeof(NotificationRoot).Assembly
        };

        builder.Services.AddSignalR();
        //builder.Services.AddJwt();
        builder.Services.AddJwtAuth();

        //persistMessage
        builder.Services.AddPersistMessageProcessor();
        //signalr connection
        builder.Services.AddSignalRConnection();

        builder.Services.AddControllers();
       
        //validation
        builder.Services.AddFluentValidation();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddCustomSwagger();
        builder.Services.AddSingleton<ISignalrHub, SignalrHub>();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
        builder.Services.AddHangfireStorageMssql();

        builder.Services.AddCustomMasstransit(
            TransportType.InMemory,
            assemblies);

        //AppDomain.CurrentDomain.GetAssemblies()
        builder.Services.AddMemoryCache();

        builder.Services.AddScoped<IEventMapper>(sp =>
        {
            var mappers = new IEventMapper[]
            {
                sp.GetRequiredService<IdentityEventMapper>(),
                sp.GetRequiredService<WalletEventMapper>(),
                sp.GetRequiredService<UserEventMapper>(),
                sp.GetRequiredService<NotificationEventMapper>()
            };
            return new CompositEventMapper(mappers);
        });

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

        app.UseCorrelationId();

        app.MapControllers();
        return app;
    }
}
