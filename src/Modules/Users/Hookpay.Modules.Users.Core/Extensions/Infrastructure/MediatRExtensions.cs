using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Modules.Users.Core.Extensions.Infrastructure;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRCustom(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UserRoot).Assembly));
        return services;
    }
}
