﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


namespace Hookpay.Shared.OpenApi;

public static class Extensions
{
    public static IServiceCollection AddSwaggerCustom(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description =
                    "Enter 'Bearer' [space] and your token in the text input below.\n\nExample: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",

            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "X-API-KEY",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
        return services;
    }

}
