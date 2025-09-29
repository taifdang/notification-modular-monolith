
using Microsoft.Extensions.DependencyInjection;

namespace Wallet.Extensions.Infrastructure;

public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(WalletRoot).Assembly));
        return services;
    }
}
