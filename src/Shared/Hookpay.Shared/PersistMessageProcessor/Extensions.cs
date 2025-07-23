
using Hookpay.Shared.Configurations;
using Hookpay.Shared.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Shared.PersistMessageProcessor;

public static class Extensions
{
    public static IServiceCollection AddPersistMessageProcessor(this IServiceCollection services)
    {
        var connectionString = services.GetOptions<MssqlOptions>("mssql").ConnectionString;

        //process after
        services.AddDbContext<PersistMessageDbContext>(x => x.UseSqlServer(connectionString));

        //if table no exist
        services.AddScoped<IPersistMessageDbContext>(
            provider =>
            {
                var persistMessageDbContext = provider.GetRequiredService<PersistMessageDbContext>();

                persistMessageDbContext.Database.EnsureCreated();
                persistMessageDbContext.CreateTableIfNotExist();    

                return persistMessageDbContext;
            });

        services.AddScoped<IPersistMessageProcessor, PersistMessageProcessor>();

        services.AddHostedService<PersistMessageProcessorBackgroundService>();

        return services;
    }
}
