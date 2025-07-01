
using Module.Sender.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShareCommon.DTO;
using System;
using System.Diagnostics.Tracing;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Module.Sender
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _provider;
        private IChannel _channel;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory provider)
        {
            _logger = logger;
            _provider = provider;         
        }    
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                var connection = await factory.CreateConnectionAsync();
                _channel = await connection.CreateChannelAsync();
                await _channel.ExchangeDeclareAsync("notification_exchange", ExchangeType.Fanout);
                await _channel.QueueDeclareAsync("push_notification_queue", true, false, false);
                await _channel.QueueBindAsync("push_notification_queue", "notification_exchange", "notification_routing_key");
                await _channel.BasicQosAsync(0, 2, false);
                await base.StartAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError($"[worker.start]::error>>{ex.ToString()}");
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {             
                    var scope = _provider.CreateScope();
                    var _sender = scope.ServiceProvider.GetRequiredService<NotificationSender>();             
                    var consumer = new AsyncEventingBasicConsumer(_channel);
                    consumer.ReceivedAsync += async (queue, message) =>
                    {
                        try
                        {
                            var msg = message.Body.ToArray();
                            var body = Encoding.UTF8.GetString(msg);
                            //send message
                            var data = JsonSerializer.Deserialize<MessagePayload>(body);
                            await _sender.HandleAsync(data);
                            _logger.LogInformation($"[received]::{body}");
                            //
                            await Task.Delay(200);
                            await _channel.BasicAckAsync(message.DeliveryTag, false);
                           
                        }
                        catch(Exception ex)
                        {
                            _logger.LogError(ex, "Xử lý message lỗi");
                        }
                    };
                    await _channel.BasicConsumeAsync("push_notification_queue", false, consumer);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation($"[error]::{ex.ToString()}");
                }              
            }
        }
    }
}
