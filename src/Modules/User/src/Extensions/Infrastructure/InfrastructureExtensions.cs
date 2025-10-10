using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using BuildingBlocks.Masstransit;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using User.Data;
using User.GrpcServer.Services;

namespace User.Extensions.Infrastructure;
public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddUserModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserEventMapper>();
        builder.Services.AddSingleton<IMasstransitModule, MasstransitExtensions>();

        builder.Services.AddValidatorsFromAssembly(typeof(UserRoot).Assembly);
        builder.Services.AddCustomDbContext<UserDbContext>();
        builder.Services.AddCustomMapster();
       
        builder.Services.AddCustomMediatR();
        builder.Services.AddGrpc();

        return builder;
    }
    public static WebApplication UseUserModules(this WebApplication app)
    {
        app.MapGrpcService<UserGrpcServices>();
        return app;
    }

}
