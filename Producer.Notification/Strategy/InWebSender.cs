using ShareCommon.DTO;
using ShareCommon.Enum;
using ShareCommon.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Producer.Notification.Strategy
{
    public class InWebSender : INotifyHandler
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<InWebSender> _logger;
        public InWebSender(IHttpClientFactory httpClient, ILogger<InWebSender> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public PushType GetAction => PushType.InWeb;

        public async Task SendAsync(MessagePayload? payload)
        {
            try
            {
                var json = JsonSerializer.Serialize(payload!.detail);
                var data = JsonSerializer.Deserialize<TopupDetail>(json);
                var client = _httpClient.CreateClient();
                await client.PostAsJsonAsync("https://localhost:7143/api/notification/push", data);
            }
            catch (Exception ex) {
                _logger.LogError($"[InWebSender]:error>>{ex.ToString()}");
            }          
        }
    }
}
