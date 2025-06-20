using Hangfire;
using Microsoft.EntityFrameworkCore;
using Producer.Notification;
using ShareCommon.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("database")));
//worker
builder.Services.AddHangfire(x =>
               x.UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(builder.Configuration.GetConnectionString("database")));
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
