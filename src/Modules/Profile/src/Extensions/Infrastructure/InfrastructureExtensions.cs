using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Profile.Data;

namespace Profile.Extensions.Infrastructure;
public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddProfileModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(ProfileRoot).Assembly);
        builder.Services.AddCustomMapster(typeof(ProfileRoot).Assembly);
        builder.Services.AddCustomDbContext<ProfileDbContext>();

        builder.Services.AddCustomMediatR();

        return builder;
    }

    public static WebApplication UseProfileModules(this WebApplication app)
    {
        //app.UseUserProfileModule();
        return app;
    }
}
