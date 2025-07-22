using Microsoft.Extensions.DependencyInjection;
using User;
namespace Hookpay.Modules.Notifications.Core.Extensions.Infrastructure;

public static class GrpcClientExtensions
{
    public static IServiceCollection AddGrpcClientCustom(this IServiceCollection services)
    {
        services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(o =>
        {
            o.Address = new Uri("https://localhost:7001");
        });
        return services;
    }
}
