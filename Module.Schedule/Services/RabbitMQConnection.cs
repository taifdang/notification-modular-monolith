using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Schedule.Services
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private IConnection? _connection;     
        public IConnection GetConnection => _connection!;

        public RabbitMQConnection()
        {
            GetInitialConnection();
        }
        public async Task GetInitialConnection()
        {
            var factory = new ConnectionFactory { HostName = "localhost"};
            _connection =  await factory.CreateConnectionAsync();     
        }      
        public void Dispose()
        {         
            _connection?.Dispose();
        }      
    }
}
