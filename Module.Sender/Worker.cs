
using Azure.Messaging;
using Microsoft.Identity.Client;
using Module.Sender.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShareCommon.DTO;
using System.Text;

namespace Module.Sender
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _provider;
        private IChannel _channel;
        private readonly MessageFill _messageFill;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory provider,MessageFill messageFill)
        {
            _logger = logger;
            _provider = provider;                
            _messageFill = messageFill;
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
                            //convert message
                            var data = System.Text.Json.JsonSerializer.Deserialize<MessagePayload>(body);
                            var payload = await GetMessageContent(data!);
                          
                            await _sender.HandleAsync(payload);
                            _logger.LogInformation($"[received]::{payload}");
                            //
                            await Task.Delay(200);
                            await _channel.BasicAckAsync(message.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Xử lý message lỗi");
                        }
                    };
                    await _channel.BasicConsumeAsync("push_notification_queue", false, consumer);
                
                }
                catch(Exception ex)
                {
                    //[retry]
                    _logger.LogInformation($"[error]::{ex.ToString()}");
                }              
            }
        }
        public  Task<NotifyPayload> GetMessageContent(MessagePayload data)
        {
            try
            {
                var content = _messageFill.MessageRender(data.event_type, data.detail);
                var payload = new NotifyPayload
                {
                    push_type = data.action,
                    send_to = data.user_id.ToString(), //user_id or email 
                    subject = data.event_type,
                    body = content,
                };
                return Task.FromResult(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{data.event_type}: {ex}");
                return null!;
            }
        }
    }
}
