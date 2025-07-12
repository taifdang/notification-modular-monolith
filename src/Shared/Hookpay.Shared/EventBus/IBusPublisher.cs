using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.EventBus;

public interface IBusPublisher
{  
    Task SendAsync<T>(T IntegrationEvent, CancellationToken cancellationToken = default);
}
