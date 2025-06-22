using ShareCommon.DTO;
using ShareCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.Strategy
{
    public class EmailSender : INotifyHandler
    {
        public PushType GetAction => PushType.Email;

        public Task SendAsync(MessagePayload? payload)
        {
            Console.WriteLine("Email send >>>");
            return Task.CompletedTask;
        }
    }
}
