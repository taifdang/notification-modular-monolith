

using Azure.Core;
using Hangfire;
using Hookpay.Modules.Topups.Api;
using Hookpay.Modules.Topups.Core;
using Hookpay.Shared;
using Hookpay.Shared.Modules;
using Hookpay.Shared.SignalR;
using Hookpay.Shared.Utils;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;



var builder = WebApplication.CreateBuilder(args).ConfigureModules();
var configuration = builder.Configuration;

var _assemblies = ModuleLoader.LoadAssemblies(configuration, "Hookpay.Modules.");
var _modules = ModuleLoader.LoadModules(_assemblies);
var tokenParameter = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = "https://hookpay.com",
    ValidAudience = "https://hookpay.com",
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))
};

builder.Services.AddModularInfrastructure(_assemblies, _modules);
foreach (var module in _modules)
{
    module.Register(builder.Services);     
}
builder.Services.AddMassTransit(x =>
{
    //x.UsingRabbitMq((context, cfg) =>
    //{
    //    cfg.Host("localhost", "/", h =>
    //    {
    //        h.Username("guest");
    //        h.Password("guest");
    //    });
    //});
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TopupRoot).Assembly));
//builder.Services.AddControllers();
//builder.Services.AddControllers().AddApplicationPart(typeof(UserRoot).Assembly);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
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

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = tokenParameter;
    });
builder.Services.AddAuthentication(nameof(TokenScheme))
    .AddJwtBearer(nameof(TokenScheme), options =>
    {
        options.Events = new JwtBearerEvents
        {     
            OnMessageReceived = (context) =>
            {
                var path = context.Request.Path;
                if (path.StartsWithSegments("/hubs"))
                {
                    var accessToken = context.Request.Query["token"];
                    if (!string.IsNullOrWhiteSpace(accessToken))
                    {
                        context.Token = accessToken;
                        Console.WriteLine($"{nameof(TokenScheme)} in Pipeline");
                    }
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = tokenParameter;
    });

builder.Services.AddAuthorization(c =>
{
    c.AddPolicy("Token", pb => pb
    .AddAuthenticationSchemes(nameof(TokenScheme))
    .RequireAuthenticatedUser());
});
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


foreach (var module in _modules)
{
    module.Use(app);
}
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapControllers();
app.MapHub<NotificationHub>("/hubs")
    .RequireAuthorization(new AuthorizeAttribute
{
    AuthenticationSchemes = nameof(TokenScheme)
});
app.MapHangfireDashboard("/hangfire");
_assemblies.Clear();
_modules.Clear();
app.Run();