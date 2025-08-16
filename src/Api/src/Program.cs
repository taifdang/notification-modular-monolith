using Api.Extensions;
using Identity.Extensions.Infrastructure;
using Notification.Extensions.Infrastructure;
using Topup.Extensions;
using UserProfile.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

builder.AddIdentityModules();
builder.AddTopupModules();
builder.AddUserProfileModules();
builder.AddNotificationModules();

var app = builder.Build();

//app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityModules();
app.UseTopupModules();
app.UseUserProfileModules();
app.UseNotificationModules();

app.UseSharedInfrastructure();

app.Run();
