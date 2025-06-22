using ShareCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.Services
{
    public class JobHandler
    {
        private readonly IJobSchedular jobSchedular;
        public JobHandler(IJobSchedular jobSchedular)
        {
            this.jobSchedular = jobSchedular;
        }
        public Task JobSender(MessagePayload? payload)
        {
            var stragy = jobSchedular.JobSenderAysnc(payload);
            return stragy.SendAsync(payload);         
        }
    }
}
