
using Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

namespace Identity.Configurations;
public class ClientAppSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public ClientAppSeeder(IServiceProvider serviceProvider)
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
            await manager.CreateAsync(Config.GetClientSeeder);
        }         
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
