
using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using BuildingBlocks.Masstransit;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Data;

namespace Wallet.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddWalletModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<WalletEventMapper>();
        builder.Services.AddSingleton<IMasstransitModule, MasstransitExtensions>();
        builder.Services.AddSingleton<IStateMachineModule, MasstransitExtensions>();

        builder.Services.AddValidatorsFromAssembly(typeof(WalletRoot).Assembly);
        builder.Services.AddCustomMapster(typeof(WalletRoot).Assembly);
        builder.Services.AddCustomDbContext<WalletDbContext>();

        builder.Services.AddCustomMediatR();

        return builder;
    }

    public static WebApplication UseWalletModules(this WebApplication app)
    {
        return app;
    }
}
