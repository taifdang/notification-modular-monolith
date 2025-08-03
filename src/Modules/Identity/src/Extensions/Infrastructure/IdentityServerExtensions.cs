
using Identity.Configurations;
using Identity.Data;
using Identity.Identity.Constants;
using Identity.Identity.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System.Net;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;
using static OpenIddict.Server.OpenIddictServerHandlers.Authentication;

namespace Identity.Extensions.Infrastructure;

//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-9.0
//ref: https://dotnettutorials.net/lesson/asp-net-core-identity-setup/
//ref: https://viblo.asia/p/authenticate-voi-identity-tren-aspnet-core-6J3Zg9RE5mB
//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0
//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0#the-identity-model
public static class IdentityServerExtensions
{
    public static WebApplicationBuilder AddIdentityServerCustom(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 10;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;
        });

        //ref: https://documentation.openiddict.com/guides/getting-started/creating-your-own-server-instance
        builder.Services.AddScoped<UserValidator>();
        builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<IdentityContext>();
            })
            .AddServer(options =>
            {
                //set endpoint
                options.SetTokenEndpointUris("/token");
                //**can be use when extenal provider / third-party => open other tab =>code, callback => signin, token
                //options.SetAuthorizationEndpointUris("/authorize");

                //set scope for server can be check
                options.RegisterScopes(
                    Scopes.Roles,
                    Scopes.Email,
                    Scopes.Profile,
                    Permissions.Prefixes.Scope + "openid",
                    Permissions.Prefixes.Scope + Constants.StandardScope.TopupApi,
                    Permissions.Prefixes.Scope + Constants.StandardScope.ProfileApi,
                    Permissions.Prefixes.Scope + Constants.StandardScope.NotificationApi,
                    Permissions.Prefixes.Scope + Constants.StandardScope.NotificationModularMonolith);

                //enable flow processor
               
                options
                    //** extenal provider / third-party
                    //.AllowAuthorizationCodeFlow()
                       .AllowClientCredentialsFlow()
                       .AllowPasswordFlow()
                       .DisableAccessTokenEncryption();

                //add certificate
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                //register host
                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough();
                          
                //debuff security for testing => eventhandler
                //options.EnableDegradedMode();

            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });


        return builder; 
    }
}
