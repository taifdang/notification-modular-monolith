using Api.Extensions;
using Identity.Extensions.Infrastructure;
using Topup.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();
builder.AddIdentityModules();
builder.AddTopupModules();

var app = builder.Build();

//app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityModules();
app.UseTopupModules();

app.UseSharedInfrastructure();

app.Run();
