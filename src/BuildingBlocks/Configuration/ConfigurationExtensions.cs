using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Configuration;

public static class ConfigurationExtensions
{
    //get service
    public static T GetOptions<T>(
        this IServiceCollection services,
        string section)
        where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        return configuration.GetOptions<T>(section);

    }

    //configuration
    public static T GetOptions<T>(
        this IConfiguration configuration,
        string section)
        where T : new()
    {
        var options = new T();

        configuration.GetSection(section).Bind(options);

        return options;
    }
}
