
using BuildingBlocks.Configuration;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EFCore;

public static class Extensions
{
    public static IServiceCollection AddMssql<T> (this IServiceCollection services)
        where T : DbContext
    {
        var options = services.GetOptions<MssqlOptions>("mssql");

        services.AddDbContext<T>(x => x.UseSqlServer(options.ConnectionString));

        return services;
    }

}
