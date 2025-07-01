using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Topup.Sevices
{
    public interface IRabbitMqProducer
    {
        Task SendMessageAsync<T>(T message);
    }
}
