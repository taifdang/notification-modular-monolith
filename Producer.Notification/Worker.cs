using Dapper;
using Microsoft.Data.SqlClient;
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
                //scan handle table cdc of outbox_table 
                //example: scan table inbox
                //#1.database connection
                using var scope = _provider.CreateScope();
                //var connection = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("database");               
                //var database = new SqlConnection(connection);
                //database.Open();
                //#2.scan table               
                //var list_data = await database.QueryAsync<InboxNotification>(
                //    "SELECT TOP 10 inotify_payload FROM inbox_notification WHERE itopup_status = @status AND inotify_created_at < @current_time",
                //    new {status = MessageStatus.Pending,current_time=DateTime.Now});
                //process
                var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var list_data = database.inbox_notification           
                    .Where(x => x.itopup_status == MessageStatus.Pending)    
                    .Select(x=>x.inotify_payload)
                    .ToList();
                foreach (var item in list_data)
                {
                    var data = JsonSerializer.Deserialize<MessagePayload>(item);                 
                    _logger.LogInformation($"[print]:data>>{data.users.user_name}");
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        public void FilterList()
        {
           
        }
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
