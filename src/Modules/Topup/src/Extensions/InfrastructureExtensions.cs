using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Topup.Data;

namespace Topup.Extensions;
public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddTopupModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMapsterCustom(typeof(TopupRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(TopupRoot).Assembly);
        builder.Services.AddMediatRCustom();

        builder.Services.AddMssql<TopupDbContext>();
        return builder;
    }

    public static WebApplication UseTopupModules(this WebApplication app)
    {
        return app;
    }

}
