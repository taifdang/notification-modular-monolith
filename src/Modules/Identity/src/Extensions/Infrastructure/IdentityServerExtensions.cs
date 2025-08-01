
using Identity.Data;
using Identity.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using System.Net;

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
        builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<IdentityContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("connect/token");
                       //.SetAuthorizationEndpointUris("connect/authorize")
                       //.SetLogoutEndpointUris("connect/logout");

                options.AllowClientCredentialsFlow().AllowRefreshTokenFlow();
                options.AllowPasswordFlow().AllowRefreshTokenFlow();

                // Encryption and signing of tokens
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate()
                       .DisableAccessTokenEncryption();

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableLogoutEndpointPassthrough()
                       .DisableTransportSecurityRequirement();

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
