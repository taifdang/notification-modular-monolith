using Newtonsoft.Json;
using ShareCommon.DTO;
using ShareCommon.Enum;
using ShareCommon.Generic;
using System.Net.Http.Json;


namespace Producer.Notification.Services
{
    public class InWebStrategy : INotifyStrategy
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<InWebStrategy> _logger;
        public PushType channel => PushType.InWeb;
        public InWebStrategy(IHttpClientFactory httpClient, ILogger<InWebStrategy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendAsync(MessagePayload? payload)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
                var data = JsonConvert.DeserializeObject<TopupDetail>(json);
                var client = _httpClient.CreateClient();
                client.BaseAddress = new Uri("https://localhost:7143/api/notification/push");
                await client.PostAsJsonAsync(client.BaseAddress, data);
            }
            catch(Exception ex)
            {
                _logger.LogError("[InWebStrategy]:error>>{Ex}",ex.ToString());
            }
        }
    }
}
