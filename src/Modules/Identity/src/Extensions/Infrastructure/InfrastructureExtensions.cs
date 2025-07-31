
using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Identity.Data;
using Microsoft.AspNetCore.Builder;

namespace Identity.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddIdentityModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMapsterCustom(typeof(IdentityRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(IdentityRoot).Assembly);
        builder.Services.AddMediatRCustom();
        //same 1 database
        builder.Services.AddMssql<IdentityContext>();
        builder.AddIdentityServerCustom();
        return builder;
    }

    public static WebApplication UseIdentityModules(this WebApplication app)
    {
        return app;
    }
}
