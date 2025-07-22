using Hookpay.Bootstrapper.Extensions;
using Hookpay.Modules.Notifications.Core.Extensions.Infrastructure;
using Hookpay.Modules.Topups.Core.Extensions.Infrastructure;
using Hookpay.Modules.Users.Core.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

builder.AddTopupModules();
builder.AddUserModules();
builder.AddNotificationModules();

var app = builder.Build();

//http into https
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseTopupModules();
app.UseUserModules();
app.UseNotificationModules();

app.UseSharedInfrastructure();

app.Run();
