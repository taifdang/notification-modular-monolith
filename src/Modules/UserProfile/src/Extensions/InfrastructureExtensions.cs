using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserProfile.Data;

namespace UserProfile.Extensions;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddUserProfileModules(this WebApplicationBuilder builder)
    {      
        builder.Services.AddMapsterCustom(typeof(UserProfileRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(UserProfileRoot).Assembly);
        builder.Services.AddMediatRCustom();

        builder.Services.AddMssql<UserProfileDbContext>();

        return builder;
    }

    public static ApplicationBuilder UseUserProfileModules(this ApplicationBuilder app)
    {

        return app;
    }
}
