using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using Microsoft.AspNetCore.Builder;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using User.GrpcServer.Services;
using User.Data;

namespace User.Extensions.Infrastructure;
public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddUserModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserEventMapper>();

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
