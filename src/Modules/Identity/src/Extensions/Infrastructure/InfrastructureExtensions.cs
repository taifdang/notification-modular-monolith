
using Microsoft.AspNetCore.Builder;

namespace Identity.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddIdentityModules(this WebApplicationBuilder builder)
    {
      
        return builder;
    }

    public static WebApplication UseIdentityModules(this WebApplication app)
    {
        return app;
    }
}
