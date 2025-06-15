using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShareCommon.Data;
using ShareCommon.DTO;
using ShareCommon.Model;
using System.Text;
using System.Text.Json;

namespace Consumer.Notification
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _provider;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory provider)
        {
            _logger = logger;
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    #region COMMAND_DB-CONNETION
                    //using var scope = _provider.CreateScope();
                    //var database = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("database");
                    //
                    #endregion
                    

                    //#rabitmq connection
                    var factory = new ConnectionFactory
                    {
                        HostName = "localhost",
                    };
                    var queueName = "notification_queue";
                    var connection = await factory.CreateConnectionAsync();
                    var channel = await connection.CreateChannelAsync();
                    //bind queue
                    await channel.QueueDeclareAsync(queueName, true, false, false, null);
                    //limit message
                    await channel.BasicQosAsync(0, 2, false);
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    //listen
                    consumer.ReceivedAsync += async (sender, eventArgs) =>
                    {
                        //convert data
                        try
                        {
                            byte[] body = eventArgs.Body.ToArray();
                            var string_data = Encoding.UTF8.GetString(body);
                            var json = JsonSerializer.Deserialize<string>(string_data);
                            var data = JsonSerializer.Deserialize<EventMessageDTO>(json!);
                            #region COMMAND
                            //add inbox tbl
                            //var inbox_notification_tbl = new InboxNotification
                            //{
                            //    event_type = data.message_type,
                            //    source = data.source,
                            //    payload = data.data,
                            //    created_at = DateTime.Now,
                            //};
                            ////THIEU TRANSACTION
                            //using var connection2 = new SqlConnection(database);
                            //connection2.Open();
                            ////message_tbl
                            //await connection2.ExecuteAsync(@"
                            //INSERT INTO messages (event_type,source,payload,created_at) 
                            //VALUES(@event_type,@source,@payload,@created_at)", inbox_notification_tbl);

                            ////inbox_tbl
                            //await connection2.ExecuteAsync(@"
                            //INSERT INTO inbox_notification (event_type,source,payload,created_at) 
                            //VALUES(@event_type,@source,@payload,@created_at)",inbox_notification_tbl);

                            //_logger.LogInformation($"[notification_service]: receive message in {data.source}");
                            //await Task.Delay(200, stoppingToken);
                            #endregion
                            //#database connection
                            using var scope = _provider.CreateScope();
                            var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                            await using var transaction = await database.Database.BeginTransactionAsync();
                            try
                            {
                                if (await database.inbox_notification.AnyAsync(x => x.message_id == Guid.Parse(eventArgs.BasicProperties.MessageId!.ToString())))
                                {
                                    _logger.LogWarning($"[receive_message]:message_id: {eventArgs.BasicProperties.MessageId!} is existed");
                                    return;
                                }
                                //#insert inbox_tbl
                                var inbox_tbl = new InboxNotification
                                {
                                    message_id = Guid.Parse(eventArgs.BasicProperties.MessageId!.ToString()),
                                    event_type = data.message_type,
                                    source = data.source,
                                    payload = data.data,
                                    created_at = DateTime.Now,
                                    status = "pending"
                                };
                                // 
                                database.inbox_notification.Add(inbox_tbl);
                                await database.SaveChangesAsync();
                                //#insert message_tbl
                                var _currentTime = DateTime.Now; 
                                var messages_tbl = new Messages
                                {
                                    event_type = inbox_tbl.event_type,
                                    source = inbox_tbl.source,
                                    payload = inbox_tbl.payload,
                                    created_at = _currentTime,
                                    message_id = eventArgs.BasicProperties?.MessageId,

                                };
                                database.messages.Add(messages_tbl);
                                //#change status inbox_tbl
                                inbox_tbl.processed_at = _currentTime;   
                                //#commit transaction 
                                await database.SaveChangesAsync();
                                await transaction.CommitAsync();

                            }
                            catch(Exception ex)
                            {
                                await transaction.RollbackAsync();
                            }
                            //
                          
                            
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex.ToString());
                        }

                        //confirm read and delete message out queue
                        //await channel.BasicAckAsync(eventArgs.DeliveryTag,false);
                    };
                    //listen
                    await channel.BasicConsumeAsync(queueName, false, consumer);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.ToString());
                }
                await Task.Delay(1000, stoppingToken);
            }         
        }
    }
}
