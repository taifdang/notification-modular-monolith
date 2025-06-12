using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Topup.Sevices
{
    public class RabbitMqConnection : IRabbitMqConnection,IDisposable
    {
        private IConnection? _connection;
        public IConnection Connection => _connection!;
        public RabbitMqConnection()
        {
            InitialConnection();
        }
        private async Task InitialConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
            };
            _connection = await factory.CreateConnectionAsync();
        }

        public void Dispose()
        {
            _connection?.Dispose();  
        }
    }
}
