using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Modules.Topups.Core.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddTopupModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMSSQL<TopupDbContext>();
        builder.Services.AddMediatRCustom();

        builder.Services.AddScoped<IBusPublisher, BusPublisher>();

        return builder;
    }

    public static WebApplication UseTopupModules(this WebApplication app)
    {
        return app;
    }
}
