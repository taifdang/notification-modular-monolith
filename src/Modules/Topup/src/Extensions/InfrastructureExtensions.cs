using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Topup.Data;
using Topup.Data.Seed;

namespace Topup.Extensions;
public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddTopupModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddCustomMapster(typeof(TopupRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(TopupRoot).Assembly);
        builder.Services.AddMediatRCustom();

        builder.Services.AddCustomDbContext<TopupDbContext>();
        builder.Services.AddScoped<IDataSeeder, TopupDataSeeder>();
        return builder;
    }

    public static WebApplication UseTopupModules(this WebApplication app)
    {
        return app;
    }

}
