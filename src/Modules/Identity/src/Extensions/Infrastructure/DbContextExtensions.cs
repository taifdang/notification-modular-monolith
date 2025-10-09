
using BuildingBlocks.Configuration;
using BuildingBlocks.EFCore;
using Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Extensions.Infrastructure;

public static class DbContextExtensions
{
    public static IServiceCollection AddCustomIdentityContext(this IServiceCollection services)
    {
        var mssqlOptions = services.GetOptions<MssqlOptions>("mssql").ConnectionString;

        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseSqlServer(mssqlOptions);

            //use schema
            options.UseOpenIddict();
        });

        //services.AddScoped<ISeedManager, SeedManager>();

        return services;
    }
}
