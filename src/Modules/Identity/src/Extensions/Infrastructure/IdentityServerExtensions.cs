
using Identity.Configurations;
using Identity.Data;
using Identity.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static OpenIddict.Server.OpenIddictServerEvents;

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
            //password settings
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 10;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;

            //lockout settings
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
                //set endpoint
                options.SetTokenEndpointUris("/connect/token");

                //set scope for server can be check
                options.RegisterScopes(Config.ApiScopes);

                //register flow 
                options.AllowPasswordFlow();

                //accept anonymous clients(i.e clients that don't send a client_id).
                options.AcceptAnonymousClients();

                //add extra config
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate()
                       .DisableAccessTokenEncryption();

                //register host
                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough();

                //add middleware, case: create token
                options.AddEventHandler<ValidateTokenRequestContext>(builder =>
                    builder.UseScopedHandler<ValidateGrantType>());

                options.AddEventHandler<ProcessSignInContext>(builder =>
                     builder.UseScopedHandler<AssignProperties>());

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
