using Microsoft.Extensions.DependencyInjection;

namespace Setting.Extensions.Infrastructure;
public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SettingRoot).Assembly));
        return services;
    }
}
