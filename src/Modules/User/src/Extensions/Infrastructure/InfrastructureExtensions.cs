using BuildingBlocks.EFCore;
using BuildingBlocks.Mapster;
using Microsoft.AspNetCore.Builder;
using FluentValidation;

namespace User.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(UserRoot).Assembly);
        builder.Services.AddCustomDbContext<Data.UserDbContext>();
        builder.Services.AddCustomMapster();
       
        builder.Services.AddCustomMediatR();

        return builder;
    }
}
