using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using ShareCommon.Data;
using ShareCommon.Model;
using System;

namespace API.Notification.Hubs
{
    public class MessageHub:Hub
    {
     
        private readonly ILogger<MessageHub> _logger;
        private readonly DatabaseContext _db;
        public MessageHub( ILogger<MessageHub> logger,DatabaseContext db)
        {         
            _logger = logger;      
            _db = db;
        }
        public async Task SendMessage(string message)
        {
            try
            {
                await Clients.All.SendAsync($"{message}_{Context.ConnectionId}");
                _logger.LogInformation($"[message_hub]:sent >> success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[message_hub]:sent >> failure");
            }
        }
        public override Task OnConnectedAsync()
        {
            try
            {
                var connection_id = Context.ConnectionId;
                //
                var hub = new HubConnection
                {
                    hub_connection_id = connection_id,
                    hub_user_name = "1",//jwt
                };
                _db.hub_connection.Add(hub);
                _db.SaveChanges();
               
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.ToString());  
            }
            return base.OnConnectedAsync();
        }
        public override  Task OnDisconnectedAsync(Exception? exception)
        {
            //remove connectionId - user
            var hubconnection = _db.hub_connection.FirstOrDefault(c => c.hub_connection_id == Context.ConnectionId);
            if (hubconnection != null)
            {
                _db.hub_connection.Remove(hubconnection);
                _db.SaveChanges();
                _logger.LogInformation($"[message_hub]:found >> connection_id: {Context.ConnectionId}");
            }
            else
            {
                _logger.LogInformation($"[message_hub]:not_found >> connection_id: {Context.ConnectionId}");
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task TestMe(string someRandomText)
        {
            await Clients.All.SendAsync(
                $"{someRandomText}_{Context.ConnectionId}",
                CancellationToken.None);
        }
        public async Task SendPersonalNotification(int user_id, string message)
        {
            try
            {
                var listHub = _db.hub_connection.FirstOrDefault(x => x.hub_user_name == user_id.ToString());
                if (listHub is null)
                {
                    _logger.LogError($"[message_hub]:error >>Not found user_id");
                    return;
                }
                else
                {
                    await Clients.Client(listHub!.hub_connection_id!).SendAsync("send person", message, CancellationToken.None);           
                    _logger.LogInformation($"[message_hub]:sent >>{listHub!.hub_connection_id!}");
                }
               
            }catch (Exception ex)
            {
                _logger.LogWarning($"[message_hub]:error >>{ex.ToString()}");
            }

        }

    }
}
