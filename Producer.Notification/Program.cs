using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Producer.Notification;
using Producer.Notification.Helper;
using Producer.Notification.Services;
using ShareCommon.Data;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHttpClient();
//builder.Services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("database")));
//worker
//builder.Services.AddHangfire(x =>
//              x.UseSimpleAssemblyNameTypeSerializer()
//              .UseRecommendedSerializerSettings()
//              .UseSqlServerStorage(builder.Configuration.GetConnectionString("database")));
//builder.Services.AddHangfireServer();
//builder.Services.AddTransient<IJobSchedular, JobScheduler>();
//builder.Services.AddTransient<IJobSchedular, JobScheduler>();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
