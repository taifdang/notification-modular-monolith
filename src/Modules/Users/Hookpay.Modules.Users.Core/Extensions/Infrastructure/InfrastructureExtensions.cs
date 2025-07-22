using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.GrpcServer.Services;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.Mapster;
using Mapster;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hookpay.Modules.Users.Core.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddUserModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddMSSQL<UserDbContext>();
        builder.Services.AddMediatRCustom();
        builder.Services.AddMapsterCustom(typeof(UserRoot).Assembly);
        //builder.Services.AddMassTransit(x =>x.AddConsumers(typeof(UserRoot).Assembly));

        //
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(UserRoot).Assembly);
        var mapperConfig = new Mapper(typeAdapterConfig);
        builder.Services.AddSingleton<IMapper>(mapperConfig);
        builder.Services.AddGrpc();

        return builder;
    }

    public static WebApplication UseUserModules(this WebApplication app)
    {
        app.MapGrpcService<UserGrpcServices>();
        return app;
    }
}
