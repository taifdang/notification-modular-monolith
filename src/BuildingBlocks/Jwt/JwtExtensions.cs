
using BuildingBlocks.Configuration;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace BuildingBlocks.Jwt;

//using JwtBearerOptions = BuildingBlocks.Web.JwtBearerOptions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services)
    {
        var connectionString = services.GetOptions<JwtBearerOptionCustom>("jwt").Key;

        //var jwtOptions = services.GetOptions<JwtBearerOptions>("JwtAuth");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {                   
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:7265",
                    ValidAudience = "https://localhost:7265",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(connectionString)),
                    ValidateLifetime = true,               
                    ValidateIssuerSigningKey = true,
                };
            });

        services.AddAuthentication(nameof(TokenSchema))
         .AddJwtBearer(nameof(TokenSchema), options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateAudience = true,
                 ValidateIssuer = true,
                 ValidIssuer = "https://localhost:7265",
                 ValidAudience = "https://localhost:7265",
                 //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(connectionString)),
                 ValidateLifetime = true,
                 //ValidateIssuerSigningKey = true,
             };

             //ref: https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-5.0#use-authorization-handlers-to-customize-hub-method-authorization            
             options.Events = new JwtBearerEvents
             {
                 OnMessageReceived = (context) =>
                 {
                     var path = context.Request.Path;

                     if (path.StartsWithSegments("/hubs"))
                     {
                         var token = context.Request.Query["Token"];

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
                    nameof(TokenSchema), 
                    policy =>
                    {
                        policy.AddAuthenticationSchemes(nameof(TokenSchema));
                        policy.RequireAuthenticatedUser();
                    });
            });


        return services;
    }
}
