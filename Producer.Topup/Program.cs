using Hangfire.Topup;
using Hangfire.Topup.Sevices;
using RabbitMQ.Client;
var builder = Host.CreateApplicationBuilder(args);
//rabbitmq
builder.Services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection());
builder.Services.AddScoped<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
