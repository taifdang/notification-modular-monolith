

using Hookpay.Modules.Topups.Api;
using Hookpay.Modules.Topups.Core;
using Hookpay.Shared;
using Hookpay.Shared.Modules;



var builder = WebApplication.CreateBuilder(args).ConfigureModules();
var configuration = builder.Configuration;


var _assemblies = ModuleLoader.LoadAssemblies(configuration, "Hookpay.Modules.");
var _modules = ModuleLoader.LoadModules(_assemblies);

builder.Services.AddModularInfrastructure(_assemblies, _modules);
foreach (var module in _modules)
{
    module.Register(builder.Services);     
}

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TopupRoot).Assembly));
//builder.Services.AddControllers();
//builder.Services.AddControllers().AddApplicationPart(typeof(UserRoot).Assembly);
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://hookpay.com";
        options.Audience = "hookpay.api";
    });
builder.Services.AddHttpContextAccessor();
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
_assemblies.Clear();
_modules.Clear();
app.Run();


