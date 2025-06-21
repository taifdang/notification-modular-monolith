using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.StrategySender
{
    public class EmailSender : INotificationSender<EmailPayload>
    {
        public Task SendAsync(EmailPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}
