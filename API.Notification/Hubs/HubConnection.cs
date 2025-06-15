using Microsoft.AspNetCore.SignalR;

namespace API.Notification.Hubs
{
    public class HubConnection:Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(message);
        }
        public override Task OnConnectedAsync()
        {
            //save connectionId - user
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //remove connectionId - user
            return base.OnDisconnectedAsync(exception);
        }
    }
}
