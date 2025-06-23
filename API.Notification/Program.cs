using API.Notification.Helper;
using API.Notification.Hubs;
using API.Notification.Repositories;
using API.Notification.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShareCommon.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Command
//builder.Services.AddSignalR();
//builder.Services.AddHangfire(x =>
//    x.UseSimpleAssemblyNameTypeSerializer()
//    .UseRecommendedSerializerSettings()
//    .UseSqlServerStorage(builder.Configuration.GetConnectionString("database"))
//);
//builder.Services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("database")), ServiceLifetime.Singleton);
//builder.Services.AddHangfireServer();
//builder.Services.AddSingleton<EventTypeDispatch>();
//builder.Services.AddSingleton<IEventHandler, TopUpHandler>();
//builder.Services.AddSingleton<MessageHub>();
//builder.Services.AddScoped<IFilterRepository, FilterRepository>();
//builder.Services.AddHostedService<SchedularWorker>();
#endregion
//
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//hangfires
app.MapHangfireDashboard("/hangfire");
app.MapControllers();
//signalr
app.MapHub<MessageHub>("/notification");

app.Run();
