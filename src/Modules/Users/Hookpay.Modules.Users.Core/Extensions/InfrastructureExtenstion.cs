using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared.EFCore;
using Mapster;
using MapsterMapper;
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
            services.AddMassTransit(x =>
                x.AddConsumers(typeof(UserRoot).Assembly)
            );
            //
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(typeof(UserRoot).Assembly);
            var mapperConfig = new Mapper(typeAdapterConfig);
            services.AddSingleton<IMapper>(mapperConfig);
            return services;
        }
       
    }
}
