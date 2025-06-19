using Consumer.Sender;
using ShareCommon.Method;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddSignalR();
//builder.Services.AddSingleton<SignalrHub>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
