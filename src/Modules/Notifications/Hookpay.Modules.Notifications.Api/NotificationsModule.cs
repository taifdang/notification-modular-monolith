using Hookpay.Modules.Notifications.Core.Extensions;
using Hookpay.Shared.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Api;

public class NotificationsModule : IModule
{
    public string Name => "Notifications";

    public IEnumerable<string> Policies => new[]
    {
        "notifications"
    };

    public void Register(IServiceCollection module)
    {
        module.AddCore();
    }

    public void Use(IApplicationBuilder app)
    {
        
    }
}
