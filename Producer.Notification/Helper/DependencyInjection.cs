using Hangfire;
using Microsoft.EntityFrameworkCore;
using Producer.Notification.Services;
using Producer.Notification.Strategy;
using Producer.Notification.StrategySender;
using ShareCommon.Data;

namespace Producer.Notification.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddSqlServerDb(services, configuration);
            AddHangfireService(services, configuration);
            AddOtherService(services, configuration);
            AddStrategySender(services, configuration);
            AddJobHandler(services, configuration);
           
            return services;
        }
        public static IServiceCollection AddHangfireService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x =>
              x.UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSqlServerStorage(configuration.GetConnectionString("database")));
            services.AddHangfireServer();
            return services;
        }
        public static IServiceCollection AddSqlServerDb(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(configuration.GetConnectionString("database")));
            return services;
        }
        public static IServiceCollection AddOtherService(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHttpClient();
            //services.AddTransient<IJobSchedular, JobScheduler>();
            return services;
        }
        public static IServiceCollection AddStrategySender(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<INotificationSender<EmailPayload>,EmailPushSender>(); 
            services.AddTransient<INotificationSender<SignalRPayload>,SignalRSender>();
            //services.AddTransient<INotifyHandler, InWebSender>();
            //services.AddTransient<INotifyHandler, EmailSender>();
            return services;
        }
        public static IServiceCollection AddJobHandler(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped<IJobSchedular, JobScheduler>();
            services.AddScoped<JobHandler>();
            services.AddScoped<INotifyHandler,EmailSender>();
            services.AddScoped<INotifyHandler,InWebSender>();
            //
            return services;
        }
        
    }
}
