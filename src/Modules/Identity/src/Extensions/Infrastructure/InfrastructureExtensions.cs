
using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Identity.Configurations;
using Identity.Data;
using Identity.Data.Seeds;
using Identity.Identity.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Identity.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddIdentityModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMapsterCustom(typeof(IdentityRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(IdentityRoot).Assembly);
        builder.Services.AddMediatRCustom();

        builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();

        builder.Services.AddScoped<UserValidator>();
        //same 1 database
        //builder.Services.AddMssql<IdentityContext>();
        builder.Services.AddIdentityContextCustom();
  
        builder.AddIdentityServerCustom();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        builder.Services.AddHostedService<ClientAppSeeder>();

        return builder;
    }

    public static WebApplication UseIdentityModules(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseMigration<IdentityContext>();

        return app;
    }
}
