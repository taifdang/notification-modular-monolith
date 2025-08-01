
using Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Configurations;
public class SeedClientAppWorker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public SeedClientAppWorker(IServiceProvider serviceProvider)
       => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if(await manager.FindByClientIdAsync("service-worker") is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "service-worker",
                ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.GrantTypes.Password
                },

            });

        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
