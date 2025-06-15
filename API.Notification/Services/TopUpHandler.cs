using API.Notification.Models;
using Microsoft.AspNetCore.Components;
using ShareCommon.DTO;
using ShareCommon.Generic;
using ShareCommon.Model;
using System.Text.Json;

namespace API.Notification.Services
{
    public class TopUpHandler : IEventHandler
    {
        public string event_type => "topup.created";    
        public Task HandleAsync(InboxNotification message)
        {
            //detect objet
            var data = JsonSerializer.Deserialize<DataPayload<Topup_Details>>(message.payload);
            //send
            Console.WriteLine($"#ID{data.entity_id} BAN DA NAP THANH CONG {data.detail.transfer_amount} ");
            return Task.CompletedTask;
        }
    }
}
