using Microsoft.Extensions.DependencyInjection;
using User;

namespace Notification.Extensions.Infrastructure;
public static class GrpcClientExtensions
{
    public static IServiceCollection AddCustomGrpcClient(this IServiceCollection services)
    {
        // not config: grpc service = lifetime scoped
        // config: grpc client = lifetime transient
        services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(o =>
        {
            o.Address = new Uri("https://localhost:7265");
        });
        return services;
    }
}
