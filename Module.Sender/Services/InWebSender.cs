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

namespace Module.Sender.Services
{
    public class InWebSender : INotifyStrategy
    {
        private readonly IHttpClientFactory _httpClient; 
        private readonly ILogger<InWebSender> _logger;
        public PushType GetPushType => PushType.InWeb;
        public InWebSender(IHttpClientFactory httpClient, ILogger<InWebSender> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task SendAsync(NotifyPayload payload)
        {
            try
            {
                //convert detail value
                //var detailValue = JsonSerializer.Serialize(payload.detail);
                //var data = JsonSerializer.Deserialize<TopupDetail>(detailValue);
                //send
                var _client = _httpClient.CreateClient();
                _client.BaseAddress = new Uri("https://localhost:7143/api/notification/push");
                await _client.PostAsJsonAsync(_client.BaseAddress, payload);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[inweb.sender]::error>>{ex.ToString()}");
            }
           
        }
    }
}
