using Module.Schedule;
using Module.Schedule.Middlewares;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServiceCollection(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
