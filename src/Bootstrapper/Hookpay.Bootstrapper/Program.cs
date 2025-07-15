

using Hookpay.Modules.Topups.Api;
using Hookpay.Modules.Topups.Core;
using Hookpay.Shared;
using Hookpay.Shared.Modules;
using Hookpay.Shared.SignalR;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;



var builder = WebApplication.CreateBuilder(args).ConfigureModules();
var configuration = builder.Configuration;

var _assemblies = ModuleLoader.LoadAssemblies(configuration, "Hookpay.Modules.");
var _modules = ModuleLoader.LoadModules(_assemblies);

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

builder.Services.AddSignalR();
//
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
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://hookpay.com",
            ValidAudience = "https://hookpay.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-key-jwt-security"))
        };
    });
builder.Services.AddAuthorization();
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
app.MapHub<NotificationHub>("/notification");
_assemblies.Clear();
_modules.Clear();
app.Run();


