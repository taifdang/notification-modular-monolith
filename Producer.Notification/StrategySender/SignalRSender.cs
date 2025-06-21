using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Notification.StrategySender
{
    public class SignalRSender : INotificationSender<SignalRPayload>
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<SignalRSender> _logger;
        public SignalRSender(IHttpClientFactory httpClient, ILogger<SignalRSender> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task SendAsync(SignalRPayload payload)
        {           
            try
            {
                var client = _httpClient.CreateClient();    
                await client.PostAsJsonAsync("https://localhost:7143/api/notification/push", payload);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"[error] >> {ex.ToString()}");
            }
        }
    }
}
