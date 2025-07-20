using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Dao;
using Hookpay.Shared.EFCore;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Modules.Users.Core.Extensions
{
    public static class InfrastructureExtenstion
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMSSQL<UserDbContext>();
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UserRoot).Assembly));
            services.AddScoped<IUserRepository,UsersRepository>();
            //         
            services.AddMassTransit(x =>
                x.AddConsumers(typeof(UserRoot).Assembly)
            );
            return services;
        }
       
    }
}
