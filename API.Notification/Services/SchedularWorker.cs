using API.Notification.Helper;
using API.Notification.Hubs;
using Dapper;
using Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.DTO;
using ShareCommon.Enum;
using ShareCommon.Generic;
using ShareCommon.Model;
using System.Text.Json;

namespace API.Notification.Services
{   
    public class SchedularWorker:BackgroundService
    {    
        private readonly IServiceScopeFactory _provider;
        //private readonly EventTypeDispatch _dispatch;
        private readonly ILogger<SchedularWorker> _logger;
       // private readonly MessageHub _messageHub;
        public SchedularWorker
            (
            IServiceScopeFactory provider, 
            //EventTypeDispatch dispatch,
            ILogger<SchedularWorker> logger
            //MessageHub messageHub
            )
        {
            _provider = provider;
            //_dispatch = dispatch;
            _logger = logger;
            //_messageHub = messageHub;
        }      
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            {
                //database connect               
                using var scope = _provider.CreateScope();            
                var sql_connection = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("database");
                //
                var signalr = scope.ServiceProvider.GetRequiredService<MessageHub>();
                var connection = new SqlConnection(sql_connection);
                connection.Open();
                try
                {
                    //get list
                    var list_inbox = connection.Query<InboxNotification>(@"SELECT TOP 10 * FROM inbox_notification WHERE itopup_status LIKE CONCAT('%',@itopup_status,'%') AND inotify_created_at < @currentDatetime", new { itopup_status = MessageStatus.Pending, currentDatetime = DateTime.Now }); //received   );                   
                    //iterator
                    foreach (var item in list_inbox)
                    {
                        //destructure
                        //await _dispatch.Dispatch(item);
                        var data = JsonSerializer.Deserialize<DataPayload<TopupDetail>>(item.inotify_payload!);
                        //payload:{push_type:inweb,content_message}
                        await signalr.SendPersonalNotification(data.user_id, $"#ID{data!.entity_id} BAN DA NAP THANH CONG {data.detail.transfer_amount}");
                        _logger.LogInformation($"[sent]: >>{data.entity_id}");
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogWarning($"[schedular]: >>{ex.ToString()}");  
                }
                await Task.Delay(5000, stoppingToken);                
            }
        }
    }
}
;