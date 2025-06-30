using Module.Schedule.Services;
using RabbitMQ.Client;
using ShareCommon.Data;
using System.Threading.Channels;

namespace Module.Schedule
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _prodvider;
        private readonly IConnectionService _connection;
        private Task _task = null!;
        public IChannel _channel;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory prodvider,IConnectionService connection)
        {
            _logger = logger;
            _prodvider = prodvider;
            _connection = connection;
            _task = GetChannelHandle();
        }

        private async Task GetChannelHandle()
        {
            try
            {
                _channel = await _connection.GetChannelAsync();
                var arguments = new Dictionary<string, object>(){
                    {"x-dead-letter-exchange","notification_exchange"},{"x-dead-letter-routing-key","notification_routing_key" }
                };
                //queue delay
                await _channel.QueueDeclareAsync("delay_notification_queue", true, false, false, arguments!);
                //exchange
                await _channel.ExchangeDeclareAsync("notification_exchange", ExchangeType.Fanout);
                await _channel.QueueDeclareAsync("push_notification_queue", true, false, false);
                await _channel.QueueBindAsync("push_notification_queue", "notification_exchange", "notification_routing_key");
            }
            catch(Exception ex)
            {
                _logger.LogError($"[Woker.Module.Schedule]::error::::{ex.ToString()}");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var scope = _prodvider.CreateScope();
                var database = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                var RabbitMQ = scope.ServiceProvider.GetRequiredService<IRabbitMQHandle>(); 
                //test
                var _messages = await database.GetMessagesAsync();
                var _filter = database.Filter(_messages);
                //schedule            
                await RabbitMQ.ScheduleAsync(_channel, _filter!);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
