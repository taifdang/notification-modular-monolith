using Api.Extensions;
using Identity.Extensions.Infrastructure;
using Notification.Extensions.Infrastructure;
using Setting.Extensions.Infrastructure;
using UserProfile.Extensions.Infrastructure;
using Wallet.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

builder.AddIdentityModules();
builder.AddWalletModules();
builder.AddUserProfileModules();
builder.AddNotificationModules();
builder.AddSettingModules();

var app = builder.Build();

//app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityModules();
app.UseWalletModules();
app.UseUserProfileModules();
app.UseNotificationModules();
app.UseSettingModules();

app.UseSharedInfrastructure();

app.Run();
