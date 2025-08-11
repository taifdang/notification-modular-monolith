using Microsoft.Extensions.DependencyInjection;

namespace UserProfile.Extensions.Infrastructure;
public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRCustom(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UserProfileRoot).Assembly));
        return services;
    }
}
