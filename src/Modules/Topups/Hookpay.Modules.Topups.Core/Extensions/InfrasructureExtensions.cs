using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Modules.Topups.Core.Extensions
{
    public static class InfrasructureExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMSSQL<TopupDbContext>();
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(TopupRoot).Assembly));
            services.AddScoped<IBusPublisher,BusPublisher>();
            return services;
        }
    }
}
