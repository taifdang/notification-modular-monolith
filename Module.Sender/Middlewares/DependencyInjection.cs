using Module.Sender.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Module.Sender.Middlewares
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services,IConfiguration configuration)
        {
            AddPushMessageStrategy(services);
            return services;
        }
        public static IServiceCollection AddPushMessageStrategy(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<INotifyStrategy,EmailSender>();
            services.AddScoped<INotifyStrategy,InWebSender>();
            services.AddScoped<INotifyFactory,NotifyFactory>();
            services.AddScoped<NotificationSender>();   
            return services;
        }
    }
}
