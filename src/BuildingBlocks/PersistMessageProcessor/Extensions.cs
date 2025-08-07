

using BuildingBlocks.Configuration;
using BuildingBlocks.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.PersistMessageProcessor;

public static class Extensions
{
    public static IServiceCollection AddPersistMessageProcessor(this IServiceCollection services)
    {

        var persistMessageOptions = services.GetOptions<MssqlOptions>("mssql");

        services.AddDbContext<PersistMessageDbContext>(x => x.UseSqlServer(persistMessageOptions.ConnectionString));

        services.AddScoped<IPersistMessageDbContext>(provider =>
        {
            var persistMessageDbContext = provider.GetRequiredService<PersistMessageDbContext>();

            //ref: https://learn.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
            persistMessageDbContext.Database.EnsureCreated();
            persistMessageDbContext.CreateTableIfNotExist();
            //if (!isExist)
            //{
            //    persistMessageDbContext.CreateTableIfNotExist();
            //}

            return persistMessageDbContext;
        });

        services.AddScoped<IPersistMessageProcessor, PersistMessageProcessor>();
   
        //services.AddHostedService<PersistMessageBackgroundService>();

        return services;
    }
}
