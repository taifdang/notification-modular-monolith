using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ShareCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Module.Schedule.Services
{
    public class RabbitMQHandle : IRabbitMQHandle
    {      
        private readonly ILogger<RabbitMQHandle> _logger;
        public RabbitMQHandle(ILogger<RabbitMQHandle> logger)
        {       
            _logger = logger;       
        }      

        public async Task ScheduleAsync(IChannel channel, IEnumerable<string> listdata)
        {
            try
            {
                foreach (var item in listdata)
                {
                    var data = JsonSerializer.Deserialize<MessagePayload>(item);
                    var body = Encoding.UTF8.GetBytes(item);
                    var timestamp = (new Random().Next(1, 10) * 1000).ToString();
                    var properties = new BasicProperties();
                    properties.MessageId = Guid.NewGuid().ToString();
                    properties.Expiration = timestamp;
                    _logger.LogError($"[sent]::{properties.MessageId}>>{timestamp}");
                    await channel.BasicPublishAsync("", "delay_notification_queue", true, properties, body);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"[rabbitmq]::error::::{ex.ToString()}");
            }
        }
    }
}
