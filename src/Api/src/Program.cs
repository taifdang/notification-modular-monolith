using Api.Extensions;
using Identity.Extensions.Infrastructure;
using Topup.Extensions;
using UserProfile.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();
builder.AddIdentityModules();
builder.AddTopupModules();
builder.AddUserProfileModules();

var app = builder.Build();

//app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityModules();
app.UseTopupModules();
app.UseUserProfileModules();

app.UseSharedInfrastructure();

app.Run();
