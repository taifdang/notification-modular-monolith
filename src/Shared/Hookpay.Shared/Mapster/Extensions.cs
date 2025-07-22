using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hookpay.Shared.Mapster;

public static class Extensions
{
    public static IServiceCollection AddMapsterCustom(this IServiceCollection services, params Assembly[] assemblies)
    {
        var typeOfAdapter = TypeAdapterConfig.GlobalSettings;
        typeOfAdapter.Scan(assemblies);
        var mapperConfig = new Mapper(typeOfAdapter);
        services.AddSingleton<IMapper>(mapperConfig);
        return services;
    }
}
