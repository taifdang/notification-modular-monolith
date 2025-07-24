using FluentValidation;
using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Shared.EFCore;
using Microsoft.AspNetCore.Builder;

namespace Hookpay.Modules.Topups.Core.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddTopupModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMSSQL<TopupDbContext>();
        builder.Services.AddMediatRCustom();
        builder.Services.AddValidatorsFromAssembly(typeof(TopupRoot).Assembly);
       
        return builder;
    }

    public static WebApplication UseTopupModules(this WebApplication app)
    {
        return app;
    }
}
