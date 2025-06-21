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
        //private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<JobScheduler> _logger;
        private readonly IEnumerable<INotificationPayload> _senders;
        public JobScheduler(
            //IHttpClientFactory httpClient,
            ILogger<JobScheduler> logger,
            IEnumerable<INotificationPayload> senders          
            ) 
        { 
            //_httpClient = httpClient;
            _logger = logger;
            _senders = senders;
        }

        public Task HandleAsync(MessagePayload? payload)
        {
            var handle = _senders.FirstOrDefault();
            throw new NotImplementedException();
        }

        //public async Task MachineSender()
        //{
        //    var message = new TopupDetail { user_id = 1, transfer_amount = 10000 };
        //    var _client = _httpClient.CreateClient();
        //    //scan handle table cdc of outbox_table 
        //    try
        //    {
        //        await _client.PostAsJsonAsync("https://localhost:7143/api/notification/push", message);
        //        _logger.LogCritical($"[post]success/send_at:{DateTime.Now}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogWarning($"[error] >> {ex.ToString()}");
        //    }
        //}
    }
}
