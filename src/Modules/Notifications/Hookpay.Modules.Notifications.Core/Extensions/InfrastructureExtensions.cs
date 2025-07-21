using Hangfire;
using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Background;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.EventBus;
using Hookpay.Shared.Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using User;


namespace Hookpay.Modules.Notifications.Core.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMSSQL<MessageDbContext>();           
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(NotificationRoot).Assembly));
        services.AddMassTransit(x => x.AddConsumers(typeof(NotificationRoot).Assembly));
        services.AddScoped<IBusPublisher, BusPublisher>();
       
        
        //
        services.AddScoped<IPersistMessageProcessor,PersistMessageProcessor>();
        services.AddTransient<HangfireMediator>();
        //
        services.AddHangfireStorageMSSQL();
        services.AddHangfireServer(options =>
        {
            options.ServerName = "localhost";
            options.Queues = new[] { "default" };
        });
        //
        services.AddScoped<ICreateMessageProcessor, CreateMesssageProcessor>();
        services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(x =>
        {
            x.Address = new Uri("https://localhost:7001");
        });        
        services.AddHostedService<PersistMesssageBackgroundService>();

        return services;
    }
    public static WebApplication UseCore(this WebApplication app)
    {      
        return app;
    }
}
