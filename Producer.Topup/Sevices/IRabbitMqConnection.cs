using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Topup.Sevices
{
    public interface IRabbitMqConnection
    {
        IConnection Connection { get; }
    }
}
