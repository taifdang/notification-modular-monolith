using FluentValidation.AspNetCore;
using Hangfire;
using Hookpay.Modules.Notifications.Core;
using Hookpay.Modules.Topups.Core;
using Hookpay.Modules.Users.Core;
using Hookpay.Shared.Caching;
using Hookpay.Shared.Core;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.EventBus;
using Hookpay.Shared.EventBus.MassTransit;
using Hookpay.Shared.Jwt;
using Hookpay.Shared.OpenApi;
using Hookpay.Shared.PersistMessageProcessor;
using Hookpay.Shared.SignalR;
using Hookpay.Shared.Utils;
using Hookpay.Shared.Web;
using MassTransit;
using Microsoft.AspNetCore.Authorization;

namespace Hookpay.Bootstrapper.Extensions;

public static class SharedInfrastructureExtensions
{
    public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
    {
             
        builder.Services.AddSignalR();     
        builder.Services.AddJwt();

        builder.Services.AddPersistMessageProcessor();

        builder.Services.AddControllers();

        //validation
        builder.Services.AddFluentValidation();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSwaggerCustom();

        builder.Services.AddSingleton<IRequestCache, RequestCache>();
        builder.Services.AddSingleton<IMessageConvert, MessageConvert>();
        builder.Services.AddSingleton<INotificationHubService, NotificationHub>();
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
        //?
        builder.Services.AddScoped<IBusPublisher, BusPublisher>();  
        
      
        builder.Services.AddHangfireStorageMSSQL();

        builder.Services.AddMassTransitCustom(AppDomain.CurrentDomain.GetAssemblies());

        //
        builder.Services.AddMemoryCache();

        builder.Services.AddScoped<IEventMapper, TopupEventMapper>();
        builder.Services.AddScoped<IEventMapper, UserEventMapper>();

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

        app.UseCorrelationId();
        //app.UseRouting();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllers();
        //});
        app.MapControllers();
        
        return app;
    }
}
