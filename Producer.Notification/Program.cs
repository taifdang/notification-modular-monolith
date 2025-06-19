using Microsoft.EntityFrameworkCore;
using Producer.Notification;
using ShareCommon.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("database")));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
