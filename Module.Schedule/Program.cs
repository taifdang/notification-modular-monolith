using Module.Schedule;
using Module.Schedule.Middlewares;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddListService(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
