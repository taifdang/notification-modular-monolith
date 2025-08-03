
using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Identity.Configurations;
using Identity.Data;
using Identity.Data.Seeds;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddIdentityModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMapsterCustom(typeof(IdentityRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(IdentityRoot).Assembly);
        builder.Services.AddMediatRCustom();

        builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();

        //
        builder.Services.AddScoped<UserValidator>();
        //same 1 database
        //builder.Services.AddMssql<IdentityContext>();
        builder.Services.AddIdentityContextCustom();

        
        builder.AddIdentityServerCustom();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        builder.Services.AddHostedService<SeedClientApp>();

        return builder;
    }

    public static WebApplication UseIdentityModules(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseMigration<IdentityContext>();

        //app.MapGet("/authorize", async (HttpContext context) =>
        //{
        //    // Resolve the claims stored in the principal created after the Steam authentication dance.
        //    // If the principal cannot be found, trigger a new challenge to redirect the user to Steam.
        //    var principal = (await context.AuthenticateAsync(SteamAuthenticationDefaults.AuthenticationScheme))?.Principal;
        //    if (principal is null)
        //    {
        //        return Results.Challenge(properties: null, [SteamAuthenticationDefaults.AuthenticationScheme]);
        //    }

        //    var identifier = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        //    // Create a new identity and import a few select claims from the Steam principal.
        //    var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType);
        //    identity.AddClaim(new Claim(Claims.Subject, identifier));
        //    identity.AddClaim(new Claim(Claims.Name, identifier).SetDestinations(Destinations.AccessToken));

        //    return Results.SignIn(new ClaimsPrincipal(identity), properties: null, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        //});
        return app;
    }
}
