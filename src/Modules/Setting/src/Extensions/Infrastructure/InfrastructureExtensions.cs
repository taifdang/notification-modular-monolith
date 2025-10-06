using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Setting.Data;

namespace Setting.Extensions.Infrastructure;
public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddSettingModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(SettingRoot).Assembly);
        builder.Services.AddCustomMapster(typeof(SettingRoot).Assembly);
        builder.Services.AddCustomDbContext<SettingDbContext>();

        builder.Services.AddCustomMediatR();

        return builder;
    }

    public static WebApplication UseSettingModules(this WebApplication app)
    {
        //app.UseUserProfileModule();
        return app;
    }
}
