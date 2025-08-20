using Microsoft.Extensions.DependencyInjection;
using UserPreference;

namespace Notification.Extensions.Infrastructure;
public static class GrpcClientExtensions
{
    public static IServiceCollection AddGrpcClientCustom(this IServiceCollection services)
    {
        //*not config: grpc service = lifetime scoped
        //*config: grpc client = lifetime transient
        services.AddGrpcClient<UserPreferenceGrpcService.UserPreferenceGrpcServiceClient>(o =>
        {
            o.Address = new Uri("https://localhost:7265");
        });
        return services;
    }
}
