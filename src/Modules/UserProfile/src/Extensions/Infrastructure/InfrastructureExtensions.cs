using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserProfile.Data;
using UserProfile.GrpcServer.Services;

namespace UserProfile.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddUserProfileModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserProfileEventMapper>();

        builder.Services.AddCustomMapster(typeof(UserProfileRoot).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(UserProfileRoot).Assembly);
        builder.Services.AddMediatRCustom();

        builder.Services.AddCustomDbContext<UserProfileDbContext>();

        builder.Services.AddGrpc();

        return builder;
    }

    public static WebApplication UseUserProfileModules(this WebApplication app)
    {
        app.MapGrpcService<UserPreferenceGrpcServices>();
        return app;
    }
}
