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
        private readonly IRabbitMQConnection _connection;
        private IChannel _channel;
        private readonly ILogger<RabbitMQHandle> _logger;
        public RabbitMQHandle(IRabbitMQConnection connection, ILogger<RabbitMQHandle> logger)
        {
            _connection = connection;          
            _logger = logger;
        }      
        public async Task GetRabbitMQConfiguration()
        {
            _channel = await _connection!.GetConnection.CreateChannelAsync();
            //delay_notify_queue
            var arguments = new Dictionary<string, object>()
            {
                //{"x-message-ttl",5000 },
                {"x-dead-letter-exchange","final_exchange"},
                {"x-dead-letter-routing-key","real_routing_key" }
            };
            await _channel.QueueDeclareAsync(queue: "delay_notify_queue", durable: true, exclusive: false, autoDelete: false,arguments:null);
            //notification_queue
            await _channel.ExchangeDeclareAsync("notification_exchange", ExchangeType.Fanout);
            await _channel.QueueDeclareAsync(queue:"notification_queue",durable:true,exclusive:false,autoDelete:false,arguments:null);
            await _channel.QueueBindAsync(queue: "notification_queue", "notification_exchange", "notification_routing_key");
        }
        public async Task Schedule(IEnumerable<string> listdata)
        {
            try
            {
                if (_connection is not null)
                {
                    await GetRabbitMQConfiguration();
                }

                foreach (var item in listdata)
                {
                    var data = JsonSerializer.Deserialize<MessagePayload>(item);
                    var work_at = MessagePayload.getWorkAt(data!.priority);
                    //schedule
                    var body = Encoding.UTF8.GetBytes(item);
                    var properties = new BasicProperties();
                    properties.Expiration = $"{work_at * 1000}";
                    await _channel.BasicPublishAsync(exchange: "", routingKey: "delay_notify_queue", mandatory: true, basicProperties: properties, body: body);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"[rabbitmq]::error::::{ex.ToString()}");
            }
        }     
    }
}
