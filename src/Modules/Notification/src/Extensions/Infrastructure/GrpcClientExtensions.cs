using Microsoft.Extensions.DependencyInjection;

namespace Notification.Extensions.Infrastructure;
public static class GrpcClientExtensions
{
    public static IServiceCollection AddGrpcClientCustom(this IServiceCollection services)
    {
        //*not config: grpc service = lifetime scoped
        //*config: grpc client = lifetime transient
        services.AddGrpcClient<UserPreference.UserPreferenceGrpcService.UserPreferenceGrpcServiceClient>(o =>
        {
            o.Address = new Uri("https://localhost:7001");
        });
        return services;
    }
}
