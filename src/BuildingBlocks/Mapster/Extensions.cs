
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Mapster;

public static class Extensions
{
    public static IServiceCollection AddCustomMapster(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var adapterConfig = TypeAdapterConfig.GlobalSettings;
        adapterConfig.Scan(assemblies);

        var mapConfig = new Mapper(adapterConfig);

        services.AddSingleton<IMapper>(mapConfig);

        return services;
    }
}
