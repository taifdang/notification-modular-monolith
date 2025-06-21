using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.StrategySender
{
    public interface INotificationSender<TPayload> where TPayload : INotificationPayload
    {
        Task SendAsync(TPayload payload);
    }
}
