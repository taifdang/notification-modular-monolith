using Api.Extensions;
using Identity.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();
builder.AddIdentityModules();

var app = builder.Build();

//app.UseDeveloperExceptionPage();

//app.UseForwardedHeaders();
//app.UseRouting();
//app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityModules();

app.UseSharedInfrastructure();

app.Run();
