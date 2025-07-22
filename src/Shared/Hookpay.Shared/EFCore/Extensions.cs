using Hangfire;
using Hookpay.Shared.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Shared.EFCore
{
    public static class Extensions
    {
        public static IServiceCollection AddMSSQL<T>(this IServiceCollection services) where T : DbContext
        {
            var options = services.GetOptions<MssqlOptions>("mssql");
            services.AddDbContext<T>(x => x.UseSqlServer(options.ConnectionString));
            return services;
        }
        public static IServiceCollection AddHangfireStorageMSSQL(this IServiceCollection services)
        {
            var options = services.GetOptions<MssqlOptions>("mssql");
            services.AddHangfire(x => x.UseSqlServerStorage(options.ConnectionString));
            return services;
        }     

    }
}
