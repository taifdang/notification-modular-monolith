using ShareCommon.DTO;
using ShareCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Sender.Services
{
    public class EmailSender : INotifyStrategy
    {
        public PushType GetPushType => PushType.Email;

        public Task SendAsync(NotifyPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}
