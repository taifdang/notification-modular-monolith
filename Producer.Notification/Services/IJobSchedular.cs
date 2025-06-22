using Producer.Notification.Strategy;
using ShareCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.Services
{
    public interface IJobSchedular
    {
        //Task MachineSender();
        Task HandleAsync(MessagePayload? payload);
        INotifyHandler JobSenderAysnc(MessagePayload? payload);
    }
}
