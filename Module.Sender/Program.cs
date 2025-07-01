using Module.Sender;
using Module.Sender.Middlewares;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServiceCollection(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
