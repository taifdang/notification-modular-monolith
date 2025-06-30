using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Schedule.Services
{
    public class ConnectionService:IConnectionService
    {
        private Task _initialTask = null!;
        private IConnection? _connection;
        public ConnectionService()
        {
            _initialTask = InitialTask();
        }     

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public async Task<IChannel> GetChannelAsync()
        {
           if(_initialTask is null) await _initialTask!;
           return await _connection!.CreateChannelAsync();
        }      
        private async Task InitialTask()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            if (_connection is not null) return;         
            _connection = await factory.CreateConnectionAsync();
        }
        
    }
}
