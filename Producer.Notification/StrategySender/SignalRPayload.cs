using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.StrategySender
{
    public record SignalRPayload(int? user_id,string content):INotificationPayload;
    
}
