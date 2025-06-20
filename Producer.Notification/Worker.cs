using Dapper;
using Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NetTopologySuite.Index.HPRtree;
using ShareCommon.Data;
using ShareCommon.DTO;
using ShareCommon.Enum;
using ShareCommon.Generic;
using ShareCommon.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
namespace Producer.Notification
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _httpClient;
        private readonly IServiceScopeFactory _provider;
        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClient, IServiceScopeFactory provider)
        {
            _logger = logger;
            _httpClient = httpClient;
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {           
            while (!stoppingToken.IsCancellationRequested)
            {
                #region command
                //scan handle table cdc of outbox_table 
                //example: scan table inbox
                //#1.database connection
                //var connection = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("database");               
                //var database = new SqlConnection(connection);
                //database.Open();
                //#2.scan table               
                //var list_data = await database.QueryAsync<InboxNotification>(
                //    "SELECT TOP 10 inotify_payload FROM inbox_notification WHERE itopup_status = @status AND inotify_created_at < @current_time",
                //    new {status = MessageStatus.Pending,current_time=DateTime.Now});
                //process
                #endregion
                using var scope = _provider.CreateScope();

                var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                //get list >>>> CDC message_tbl
                var list_data = database.messages.ToList();//[???]
                //>> filter user_is_block and !enable_notification            
                var filter_query = list_data.
                    Join(database.users,
                        message => message.mess_user_id,
                        user => user.user_id,
                        (message, user) => new { message, user }
                    )
                    .Join(database.settings,
                        match => match.user.user_id,
                        settings => settings.set_user_id,
                        (match, settings) => new { match.user, match.message, settings }
                    )
                    .Where(x => x.settings.disable_notification == false && x.user.is_block == false)
                    .Select(x => x.message.mess_payload);
                //>> schedular priority, send_time, save data in outbox_tbl
                foreach (var item in filter_query)
                {
                    //>> get priority set send time >>> scheduler 
                    var data = JsonSerializer.Deserialize<MessagePayload>(item);                              
                    _logger.LogCritical($"[worker]:data>>{data.priority}");
                    //BackgroundJob.Schedule<Class>
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        //Scheduler via priority + hangfire   
        public async Task PostSender()
        {
            var message = new TopupDetail { user_id = 1, transfer_amount = 10000 };
            var _client = _httpClient.CreateClient();
            //scan handle table cdc of outbox_table 
            try
            {
                await _client.PostAsJsonAsync("https://localhost:7143/api/notification/push", message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"[error] >> {ex.ToString()}");
            }
        }
        public Task SchedularMessage()
        {
            return Task.CompletedTask;
        }
    }
}
