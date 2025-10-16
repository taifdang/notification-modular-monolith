using Api.Extensions;
using Identity.Extensions.Infrastructure;
using Notification.Infrastructure.Extensions;
using User.Extensions.Infrastructure;
using Wallet.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

builder.AddIdentityModules();
builder.AddWalletModules();
builder.AddNotificationModules();
builder.AddUserModules();

var app = builder.Build();

//app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityModules();
app.UseWalletModules();
app.UseNotificationModules();
app.UseUserModules();

app.UseSharedInfrastructure();

app.Run();
