using ShareCommon.DTO;
using ShareCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Sender.Services
{
    public interface INotifyStrategy
    {
        PushType GetPushType { get; }
        Task SendAsync(NotifyPayload payload);
    }
}
