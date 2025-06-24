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
                    //create queue if queue not exist
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
                            //var data = JsonSerializer.Deserialize<EventMessageDTO>(json!);
                            //###
                            var data = JsonSerializer.Deserialize<EventMess>(json);
                            //
                            #region COMMAND
                            //add inbox tbl
                            //var inbox_notification_tbl = new InboxNotification
                            //{
                            //    otopup_event_type = data.message_type,
                            //    topup_source = data.topup_source,
                            //    otopup_payload = data.data,
                            //    otopup_created_at = DateTime.Now,
                            //};
                            ////THIEU TRANSACTION
                            //using var connection2 = new SqlConnection(database);
                            //connection2.Open();
                            ////message_tbl
                            //await connection2.ExecuteAsync(@"
                            //INSERT INTO messages (otopup_event_type,topup_source,otopup_payload,otopup_created_at) 
                            //VALUES(@otopup_event_type,@topup_source,@otopup_payload,@otopup_created_at)", inbox_notification_tbl);

                            ////inbox_tbl
                            //await connection2.ExecuteAsync(@"
                            //INSERT INTO inbox_notification (otopup_event_type,topup_source,otopup_payload,otopup_created_at) 
                            //VALUES(@otopup_event_type,@topup_source,@otopup_payload,@otopup_created_at)",inbox_notification_tbl);

                            //_logger.LogInformation($"[notification_service]: receive message in {data.topup_source}");
                            //await Task.Delay(200, stoppingToken);
                            #endregion
                            //#database connection
                            using var scope = _provider.CreateScope();
                            var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                            await using var transaction = await database.Database.BeginTransactionAsync();
                            try
                            {
                                if (await database.inbox_notification.AnyAsync(x => x.inotify_id == Guid.Parse(eventArgs.BasicProperties.MessageId!.ToString())))
                                {
                                    _logger.LogWarning($"[receive_message]:message_id: {eventArgs.BasicProperties.MessageId!} is existed");
                                    return;
                                }
                                //#insert inbox_tbl
                                var inbox_tbl = new InboxNotification
                                {
                                    inotify_id = Guid.Parse(eventArgs.BasicProperties.MessageId!.ToString()),
                                    inotify_event_type = data!.message_type!,
                                    inotify_source = data.source,
                                    inotify_payload = JsonSerializer.Serialize(data.data),
                                    inotify_created_at = DateTime.Now,                                    
                                    //status = "pending"
                                };
                                // 
                                database.inbox_notification.Add(inbox_tbl);
                                await database.SaveChangesAsync();
                                //#insert message_tbl
                                var _currentTime = DateTime.Now; 
                                var messages_tbl = new Messages
                                {
                                    mess_event_type = inbox_tbl.inotify_event_type!,
                                    mess_source = inbox_tbl.inotify_source,
                                    mess_payload = inbox_tbl.inotify_payload,
                                    mess_created_at = _currentTime,
                                    mess_event_id = Guid.Parse(eventArgs.BasicProperties?.MessageId!),
                                    mess_user_id = data.data.user_id,
                                };
                                database.messages.Add(messages_tbl);
                                //#change otopup_status inbox_tbl
                                inbox_tbl.inotify_updated_at = _currentTime;
                                inbox_tbl.itopup_status = ShareCommon.Enum.MessageStatus.Pending;
                                //#commit transaction 
                                await database.SaveChangesAsync();
                                await transaction.CommitAsync();

                            }
                            catch(Exception ex)
                            {
                                await transaction.RollbackAsync();
                                _logger.LogWarning(ex.ToString());  
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
