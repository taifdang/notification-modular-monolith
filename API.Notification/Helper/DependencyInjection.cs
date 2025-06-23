using API.Notification.Hubs;
using API.Notification.Repositories;
using API.Notification.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using System.Threading.Tasks;

namespace API.Notification.Helper
{   
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
        {
            AddSignalRService(services);
            AddSqlServerDb(services, configuration);
            AddHangfire(services, configuration);       
            AddWorkerService(services);
            return services;
        }
        public static IServiceCollection AddSqlServerDb(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(configuration.GetConnectionString("database")),ServiceLifetime.Singleton);
            return services;
        }
        public static IServiceCollection AddHangfire(IServiceCollection services,IConfiguration configuration)
        {
            services.AddHangfire(x =>x
                //.UseSimpleAssemblyNameTypeSerializer()
                //.UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("database")));
            //services.AddHangfireServer();
            return services;    
        }
        public static IServiceCollection AddSignalRService(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<MessageHub>();
            return services;
        }
        public static IServiceCollection AddOthersDI(IServiceCollection services)
        {
            services.AddSingleton<EventTypeDispatch>();
            services.AddSingleton<IEventHandler, TopUpHandler>();
            //
            services.AddScoped<IFilterRepository, FilterRepository>();
            return services;
        }
        public static IServiceCollection AddWorkerService(IServiceCollection services)
        {
            //services.AddHostedService<SchedularWorker>();
            return services;
        }
    } 
}
