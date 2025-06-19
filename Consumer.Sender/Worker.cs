using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShareCommon.DTO;
using ShareCommon.Generic;
using ShareCommon.Method;
using ShareCommon.Model;
using System.Text.Json;

namespace Consumer.Sender
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _provider;      
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<SignalrHub> hubContext;
        // private readonly MessageHub _messageHub;
        public Worker
            (
            IServiceScopeFactory provider,
         
            ILogger<Worker> logger,
            IHubContext<SignalrHub> hubContext
            //MessageHub messageHub
            )
        {
            _provider = provider;
          
            _logger = logger;
            this.hubContext = hubContext;
            //_messageHub = messageHub;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //database connect
                //using var scope = _provider.CreateScope();              
                //var signalr = scope.ServiceProvider.GetRequiredService<SignalrHub>();
             
                try
                {
                    //iterator
                    //destructure
                        var json = "{\"entity_id\":4,\"action\":\"inapp\",\"status\":\"waiting\",\"user_id\":1,\"detail\":{\"username\":\"taidang01\",\"transfer_amount\":10000},\"priority\":\"high\"}";
                        var data = JsonSerializer.Deserialize<DataPayload<TopupDetail>>(json);
                    //payload:{push_type:inweb,content_message}
                    await hubContext.Clients.All.SendAsync($"#ID{data!.entity_id} BAN DA NAP THANH CONG {data.detail.transfer_amount}",CancellationToken.None);        
                    
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"[schedular]: >>{ex.ToString()}");
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
