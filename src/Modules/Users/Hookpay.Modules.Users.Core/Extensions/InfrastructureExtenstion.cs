using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.GrpcServer.Services;
using Hookpay.Shared.EFCore;
using Mapster;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Builder;
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
            services.AddGrpc();
            //
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(typeof(UserRoot).Assembly);
            var mapperConfig = new Mapper(typeAdapterConfig);
            services.AddSingleton<IMapper>(mapperConfig);
            return services;
        }
        public static WebApplication UseCore(this WebApplication app)
        {
            app.MapGrpcService<UserGrpcServices>();
            return app;
        }

    }
}
