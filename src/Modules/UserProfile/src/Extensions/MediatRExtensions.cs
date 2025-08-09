using Microsoft.Extensions.DependencyInjection;

namespace UserProfile.Extensions;
public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRCustom(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UserProfileRoot).Assembly));
        return services;
    }
}
