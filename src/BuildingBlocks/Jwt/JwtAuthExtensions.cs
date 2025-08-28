using BuildingBlocks.Configuration;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Jwt;
//ref: https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-5.0#use-authorization-handlers-to-customize-hub-method-authorization
public static class JwtAuthExtensions
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services)
    {
        var jwtOptions = services.GetOptions<JwtBearerOptions>("JwtAuth");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = jwtOptions.Authority;
            options.Audience = jwtOptions.Audience;
            options.RequireHttpsMetadata = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuers = [jwtOptions.Authority, "https://localhost:7265/"],
                ValidateAudience = true,
                ValidAudiences = [jwtOptions.Audience],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(2),                                                  
                ValidateIssuerSigningKey = true,
                NameClaimType = "name", // Map "name" claim to User.Identity.Name
                RoleClaimType = "role", // Map "role" claim to User.IsInRole()
            };
            options.MapInboundClaims = false;

            //signalr check token
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = (context) =>
                {
                    var path = context.Request.Path;

                    if (path.StartsWithSegments("/hubs"))
                    {
                        var token = context.Request.Query["token"];

                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            context.Token = token;
                        }
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(
               options =>
               {
                   options.AddPolicy(
                       nameof(ApiScope),
                       policy =>
                       {
                           policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                           policy.RequireAuthenticatedUser();
                           policy.RequireClaim("scope", jwtOptions.Audience);
                       });
                   options.AddPolicy(
                       nameof(SignalrSchema),
                       policy =>
                       {
                           policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                           policy.RequireAuthenticatedUser();
                       });
               });

        return services;
    }
}
