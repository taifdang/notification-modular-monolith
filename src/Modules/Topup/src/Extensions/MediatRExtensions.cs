using Microsoft.Extensions.DependencyInjection;

namespace Topup.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRCustom(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(TopupRoot).Assembly));
        return services;
    }
}
