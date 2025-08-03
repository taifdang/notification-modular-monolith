
using Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using Identity.Identity.Constants;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Configurations;
public class SeedClientApp : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public SeedClientApp(IServiceProvider serviceProvider)
       => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        if (await manager.FindByClientIdAsync("client") is null)
        {
            //ref: https://documentation.openiddict.com/configuration/application-permissions
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "client",
                ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                //confidential require client_id, client_secret
                ClientType = ClientTypes.Confidential,
                //register enable uri
                RedirectUris =
                {
                    new Uri("https://localhost:7265/token")
                },
                Permissions =
                {
                    //endpoint processor
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.Authorization,

                    //grant_type
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.GrantTypes.Password,

                    //response_type
                    Permissions.ResponseTypes.Code,
                    //command: OIDC - openId
                    Permissions.ResponseTypes.IdToken,

                    //scope
                    Permissions.Scopes.Roles,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,

                    //custom scope
                    Permissions.Prefixes.Scope + "openid",
                    Permissions.Prefixes.Scope + Constants.StandardScope.TopupApi,
                    Permissions.Prefixes.Scope + Constants.StandardScope.ProfileApi,
                    Permissions.Prefixes.Scope + Constants.StandardScope.NotificationApi,
                    Permissions.Prefixes.Scope + Constants.StandardScope.NotificationModularMonolith
                },
            });

        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
