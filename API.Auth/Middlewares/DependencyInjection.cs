using API.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShareCommon.Data;
using StackExchange.Redis;
using System.Text;

namespace API.Auth.Middlewares
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSevicesCollection(this IServiceCollection services,IConfiguration configuration)
        {
            AddMSSQL(services, configuration);
            AddJWTBearer(services, configuration);
            AddRedisCache(services, configuration); 
            
            return services;
        }
        public static IServiceCollection AddMSSQL(IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(configuration.GetConnectionString("database")));
            return services;
        }
        public static IServiceCollection AddJWTBearer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["hostname"],
                        ValidAudience = configuration["hostname"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                    };
                });
            services.AddScoped<ICustomerService, CustomerService>();
            return services;
        }
        public static IServiceCollection AddRedisCache(IServiceCollection services,IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("redis")!;
            });
            //services.AddSingleton<IConnectionMultiplexer>(options =>
            //     ConnectionMultiplexer.Connect(configuration.GetConnectionString("redis")!));
            return services;
        }

    }
}
