using Microsoft.Extensions.DependencyInjection;

namespace Profile.Extensions.Infrastructure;

public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProfileRoot).Assembly));
        return services;
    }
}
