using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Dao;
using Hookpay.Shared.EFCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core
{
    public static class Extenstions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMSSQL<UserDbContext>();
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UserRoot).Assembly));
            services.AddScoped<IUserRepository,UsersRepository>();
            return services;
        }
    }
}
