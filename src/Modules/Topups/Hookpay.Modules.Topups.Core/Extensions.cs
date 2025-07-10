using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Modules.Topups.Core.Topups.Dao;
using Hookpay.Shared.EFCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMSSQL<TopupDbContext>();
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(TopupRoot).Assembly));
            services.AddScoped<ITopupRepository, TopupRepository>();
            return services;
        }
    }
}
