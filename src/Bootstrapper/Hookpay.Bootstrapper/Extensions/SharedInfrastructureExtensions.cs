using Hangfire;
using Hookpay.Modules.Notifications.Core;
using Hookpay.Modules.Topups.Core;
using Hookpay.Modules.Users.Core;
using Hookpay.Shared.Caching;
using Hookpay.Shared.Core;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.Jwt;
using Hookpay.Shared.OpenApi;
using Hookpay.Shared.PersistMessageProcessor;
using Hookpay.Shared.SignalR;
using Hookpay.Shared.Utils;
using MassTransit;
using Microsoft.AspNetCore.Authorization;

namespace Hookpay.Bootstrapper.Extensions;

public static class SharedInfrastructureExtensions
{
    public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
    {
        

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSignalR();     
        builder.Services.AddJwt();
        builder.Services.AddMemoryCache();

        builder.Services.AddControllers();
        builder.Services.AddSwaggerCustom();

        builder.Services.AddSingleton<IRequestCache, RequestCache>();
        builder.Services.AddSingleton<IMessageConvert, MessageConvert>();
        builder.Services.AddSingleton<INotificationHubService, NotificationHub>();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
        

        builder.Services.AddPersistMessageProcessor();
        
        builder.Services.AddHangfireStorageMSSQL();

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(UserRoot).Assembly);
            x.AddConsumers(typeof(NotificationRoot).Assembly);

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        //
        //
        builder.Services.AddScoped<IEventMapper, TopupEventMapper>();


        return builder;
    }

    public static WebApplication UseSharedInfrastructure(this WebApplication app)
    {      
        app.MapHub<NotificationHub>("/hubs")
            .RequireAuthorization(new AuthorizeAttribute
            {
                AuthenticationSchemes = nameof(TokenScheme)
            });

        app.MapHangfireDashboard("/hangfire");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }        
        //app.UseRouting();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllers();
        //});
        app.MapControllers();
        
        return app;
    }
}
