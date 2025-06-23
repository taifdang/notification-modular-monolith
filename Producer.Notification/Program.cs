using Producer.Notification;
using Producer.Notification.Helper;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
