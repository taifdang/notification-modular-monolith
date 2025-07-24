using FluentValidation;
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
        builder.Services.AddValidatorsFromAssembly(typeof(UserRoot).Assembly);
        builder.Services.AddGrpc();

        return builder;
    }

    public static WebApplication UseUserModules(this WebApplication app)
    {
        app.MapGrpcService<UserGrpcServices>();
        return app;
    }
}
