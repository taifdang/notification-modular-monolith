using Dapper;
using Microsoft.Data.SqlClient;
using Producer.Topup.Sevices;
using ShareCommon.Model;

namespace Producer.Topup
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _provider;
        public Worker(ILogger<Worker> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var _scope = _provider.CreateScope();
                var db = _scope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("database");
                var rabbitMq = _scope.ServiceProvider.GetRequiredService<IRabbitMqProducer>();
                //
                using var connection = new SqlConnection(db);
                await connection.OpenAsync();
                await using var transaction = await connection.BeginTransactionAsync();
                //
                var _topup_msg = await connection.QueryAsync<OutboxTopup>(@"
                    SELECT TOP 10 id,payload 
                    FROM outbox_topup
                    WHERE processed_at IS NULL
                ", transaction: transaction);
                if(_topup_msg is not null) 
                {
                    foreach (var item in _topup_msg)
                    {
                        try
                        {
                            //push message in queue
                            await rabbitMq.SendMessageAsync(item.payload);
                            //
                            await connection.ExecuteAsync(
                               @"
                                    UPDATE outbox_topup
                                    SET processed_at = @processCurrent
                                    WHERE id = @Id        
                                ",
                               new { processCurrent = DateTime.Now, Id = item.id },
                               transaction: transaction);
                            //Log
                            _logger.LogInformation("[topup_producer]: changed topup item");
                        }
                        catch(Exception ex)
                        {
                            await connection.ExecuteAsync(
                                @"
                                    UPDATE outbox_topup
                                    SET processed_at = @processCurrent, error = @Error
                                    WHERE id = @Id
                                ",
                                new { processCurrent = DateTime.Now, Error = ex.ToString(), Id = item.id },
                                transaction: transaction);
                            _logger.LogInformation("[topup_producer]: send message occur error");
                        }
                    }
                }
                await transaction.CommitAsync();
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
