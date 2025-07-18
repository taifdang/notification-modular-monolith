using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.SignalR;

public interface INotificationHubService
{
    Task SendAllAsync(string message);
    Task SendPersonalAsync(string userId, string message);
}
