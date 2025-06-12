using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Producer.Topup.Sevices
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly IRabbitMqConnection _connection;
        public RabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }
        public async Task SendMessageAsync<T>(T message)
        {
            try
            {
                using var _channel = await _connection.Connection.CreateChannelAsync();
                var routingKey = "notification_queue";
                var props = new BasicProperties();
                //confirm queue
                await _channel.QueueDeclareAsync(routingKey, true, false, false, null);
                //convert
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);
                //push messages
                await _channel.BasicPublishAsync("", routingKey, true, props, body);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
