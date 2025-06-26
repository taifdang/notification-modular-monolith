using Module.Schedule.Services;
using ShareCommon.Data;

namespace Module.Schedule
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _prodvider;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory prodvider)
        {
            _logger = logger;
            _prodvider = prodvider;
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
                await RabbitMQ.Schedule(_filter!);              
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
