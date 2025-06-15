using API.Notification.Helper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.Model;

namespace API.Notification.Services
{
    public class SchedularWorker:BackgroundService
    {    
        private readonly IServiceScopeFactory _provider;
        private readonly EventTypeDispatch _dispatch;
        public SchedularWorker(IServiceScopeFactory provider, EventTypeDispatch dispatch)
        {
            _provider = provider;
            _dispatch = dispatch;
        }      
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {              
                //database connect
                using var scope = _provider.CreateScope();            
                var sql_connection = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("database");
                var connection = new SqlConnection(sql_connection);
                connection.Open();
                //get list
                var list_inbox = connection.Query<InboxNotification>(
                    @"SELECT TOP 10 * FROM inbox_notification WHERE status LIKE CONCAT('%',@status,'%')", new {status = "pending"}    
                );
                //iterator
                foreach (var item in list_inbox)
                {
                    await _dispatch.Dispatch(item);
                }
                await Task.Delay(5000, stoppingToken);                
            }
        }
    }
}
