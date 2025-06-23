
using Hangfire;
using Producer.Notification.Services;
using ShareCommon.Data;
using ShareCommon.DTO;
using System.Text.Json;


namespace Producer.Notification
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
     
        private readonly IServiceScopeFactory _provider;
        private readonly IBackgroundJobClient _jobClient;       
        public Worker(ILogger<Worker> logger,IServiceScopeFactory provider,IBackgroundJobClient jobClient)
        {
            _logger = logger;
           
            _provider = provider;
            _jobClient = jobClient;
               
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {           
            while (!stoppingToken.IsCancellationRequested)
            {
                try
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
                    var filter_query = list_data
                        .Join(database.users,
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
                    #region process
                    //>> schedular priority, send_time, save data in outbox_tbl
                    //foreach (var item in filter_query)
                    //{
                    //    //>> get priority set send time >>> scheduler 
                    //    var data = JsonSerializer.Deserialize<MessagePayload>(item);                              
                    //    _logger.LogCritical($"[worker]:data>>{data.priority}");
                    //    //
                    //    //var job = scope.ServiceProvider.GetRequiredService<IJobSchedular>();
                    //    //_backgroundJobClient.Schedule<IJobSchedular>(x=>x.MachinXeSender(),TimeSpan.FromSeconds(0));
                    //    //
                    //    //_backgroundJobClient.Schedule<IJobSchedular>(x=>x.MachineSender(), TimeSpan.FromSeconds(0));
                    //    //_logger.LogCritical("[end-sending]");
                    //}
                    #endregion
                   
                    await ScheduleHandler(filter_query);                   
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[worker.notification]:error >> {ex.ToString()}");
                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }      
        public async Task ScheduleHandler(IEnumerable<string> jobs)
        {
            foreach (var item in jobs)
            {
                var data = JsonSerializer.Deserialize<MessagePayload>(item);
                var work_at = MessagePayload.getWorkAt(data.priority);
                //await ExcuteAsync(data); //test
                _jobClient.Schedule(() => ExcuteAsync(data),TimeSpan.FromSeconds(work_at));
            }
        }
        public async Task ExcuteAsync(MessagePayload payload)
        {
            using var scope = _provider.CreateScope();
            var excute = scope.ServiceProvider.GetService<NotificationSender>();
            await excute.HandleAsync(payload);
        }
    }
}
