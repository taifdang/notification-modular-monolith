
using Hookpay.Shared.Configurations;
using Hookpay.Shared.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

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

                /*
                var creator = (RelationalDatabaseCreator)persistMessageDbContext.GetService<IDatabaseCreator>();
                if (!creator.HasTables())
                {
                   //logic
                }
                */

                persistMessageDbContext.Database.EnsureCreated();
                persistMessageDbContext.CreateTableIfNotExist();

                return persistMessageDbContext;
            });

        services.AddScoped<IPersistMessageProcessor, PersistMessageProcessor>();

        services.AddHostedService<PersistMessageProcessorBackgroundService>();

        return services;
    }
}
