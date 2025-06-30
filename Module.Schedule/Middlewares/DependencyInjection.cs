using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Module.Schedule.Services;
using ShareCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Schedule.Middlewares
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services,IConfiguration configuration)
        {
            AddMSSQLDB(services, configuration);
            AddScheduleHandle(services);
            AddRabbitMQConnection(services);
            AddRabbitMQAdapter(services);
            return services;
        }
        public static IServiceCollection AddMSSQLDB(IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(configuration.GetConnectionString("database")),ServiceLifetime.Scoped);
            return services;
        }
        public static IServiceCollection AddScheduleHandle(IServiceCollection services)
        {
            services.AddScoped<IMessageRepository, MessageRepository>();
            return services;
        }
        public static IServiceCollection AddRabbitMQConnection(IServiceCollection services)
        {
            //services.AddSingleton<IRabbitMQConnection>(new RabbitMQConnection());
            services.AddScoped<IRabbitMQHandle, RabbitMQHandle>();
            return services;
        }
        public static IServiceCollection AddRabbitMQAdapter(IServiceCollection services)
        {
            services.TryAddSingleton<IConnectionService>(new ConnectionService());
            return services;
        }
    }
}
