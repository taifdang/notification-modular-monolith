using Producer.Notification.Strategy;
using Producer.Notification.StrategySender;
using ShareCommon.DTO;
using ShareCommon.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.Services
{
    public class JobScheduler:IJobSchedular
    {     
        private readonly IEnumerable<INotifyHandler> _senders;
        public JobScheduler(IEnumerable<INotifyHandler> senders) 
        { 
            _senders = senders;
        }

        public async Task HandleAsync(MessagePayload? payload)
        {
            var handle = _senders.FirstOrDefault(x=>x.GetAction == payload!.action);
            await handle!.SendAsync(payload);
        }

        public INotifyHandler JobSenderAysnc(MessagePayload? payload)
        {
            var handle = _senders.FirstOrDefault(x => x.GetAction == payload!.action);
            return handle!;
        }
    }
}
