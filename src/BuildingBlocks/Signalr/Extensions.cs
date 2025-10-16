using BuildingBlocks.Configuration;
using BuildingBlocks.EFCore;
using BuildingBlocks.PersistMessageProcessor;
using BuildingBlocks.Signalr.Repository;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Signalr;

public static class Extensions
{
    public static IServiceCollection AddSignalRConnection(this IServiceCollection services)
    {
        var connectionsOptions = services.GetOptions<MssqlOptions>("mssql");

        services.AddDbContext<ConnectionDbContext>(x => x.UseSqlServer(connectionsOptions.ConnectionString));

        services.AddScoped<IConnectionDbContext>(provider =>
        {
            var connectionDbContext = provider.GetRequiredService<ConnectionDbContext>();
            if (connectionDbContext.Database.EnsureCreated())
            {
                connectionDbContext.CreateTableIfNotExist();
            }         
            return connectionDbContext;
        });

        services.AddScoped<IConnectionProcessor, ConnectionProcessor>();

        return services;
    }
}
