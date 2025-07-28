using Hookpay.Shared.Configurations;
using Hookpay.Shared.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hookpay.Shared.Jwt;

public static class JwtExtensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services)
    {
        var JwtOptions = services.GetOptions<EFCore.JwtBearerOptions>("jwt").Key;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {              
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = "https://hookpay.com",
                    ValidAudience = "https://hookpay.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions)),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

        services.AddAuthentication(nameof(TokenScheme))
            .AddJwtBearer(nameof(TokenScheme), options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = "https://hookpay.com",
                    ValidAudience = "https://hookpay.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions)),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };

                //ref: https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-5.0#use-authorization-handlers-to-customize-hub-method-authorization
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        var path = context.Request.Path;
                        if (path.StartsWithSegments("/hubs"))
                        {
                            var accessToken = context.Request.Query["token"];
                            if (!string.IsNullOrWhiteSpace(accessToken))
                            {
                                context.Token = accessToken;                           
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(TokenScheme), policy => policy
                .AddAuthenticationSchemes(nameof(TokenScheme))
                .RequireAuthenticatedUser()
            );
        });

        return services;
    }
}
