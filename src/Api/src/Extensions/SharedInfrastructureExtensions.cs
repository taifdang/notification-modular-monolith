using BuildingBlocks.Hangfire;
using BuildingBlocks.Jwt;
using BuildingBlocks.OpenApi;
using BuildingBlocks.Web;
using FluentValidation.AspNetCore;
using Hangfire;

namespace Api.Extensions;

public static class SharedInfrastructureExtensions
{
    public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        builder.Services.AddJwt();

        builder.Services.AddControllers();

        //validation
        builder.Services.AddFluentValidation();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSwaggerCustom();

        builder.Services.AddHangfireStorageMssql();

        builder.Services.AddMemoryCache();


        return builder;
    }

    public static WebApplication UseSharedInfrastructure(this WebApplication app)
    {
        

        app.MapHangfireDashboard("/hangfire");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCorrelationId();

        app.MapControllers();

        return app;
    }
}
