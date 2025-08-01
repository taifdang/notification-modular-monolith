
using BuildingBlocks.Configuration;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.EFCore;

public static class Extensions
{
    public static IServiceCollection AddMssql<T> (this IServiceCollection services)
        where T : DbContext, IDbContext
    {
        var options = services.GetOptions<MssqlOptions>("mssql");

        services.AddDbContext<T>(x => x.UseSqlServer(options.ConnectionString));
     
        return services;
    }

    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
    where TContext : DbContext, IDbContext
    {
        MigrateAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

        SeedAsync(app.ApplicationServices).GetAwaiter().GetResult();

        return app;
    }

    private static async Task MigrateAsync<TContext>(IServiceProvider serviceProvider)
   where TContext : DbContext, IDbContext
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());

            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");
        }
    }

    private static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var seedersManager = scope.ServiceProvider.GetRequiredService<ISeedManager>();

        await seedersManager.ExecuteSeedAsync();
    }

}
