using Hangfire;
using Microsoft.EntityFrameworkCore;
using Producer.Notification.Services;
using ShareCommon.Data;

namespace Producer.Notification.Middlewares
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddSqlServerDb(services, configuration);
            AddHangfireService(services, configuration);          
            AddNotifyStrategy(services);
            return services;
        }
        public static IServiceCollection AddHangfireService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x =>x
              //.UseSimpleAssemblyNameTypeSerializer()
              //.UseRecommendedSerializerSettings()
              .UseSqlServerStorage(configuration.GetConnectionString("database")));
            services.AddHangfireServer(options =>
            {
                options.ServerName = "my-worker"; 
                options.Queues = new[] { "default" };
            });
            return services;
        }
        public static IServiceCollection AddSqlServerDb(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(configuration.GetConnectionString("database")));
            return services;
        }           
        public static IServiceCollection AddNotifyStrategy(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<INotifyFactory, NotifyFactory>();
            services.AddScoped<INotifyStrategy, InWebStrategy>();
            services.AddScoped<INotifyStrategy, EmailStrategy>();
            services.AddScoped<NotificationSender>();

            return services;
        }
        
    }
}
