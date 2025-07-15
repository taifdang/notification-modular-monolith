using Hookpay.Shared.Contracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public class SendAllMessage : IConsumer<MessageAllContracts>
{
    public Task Consume(ConsumeContext<MessageAllContracts> context)
    {
        throw new NotImplementedException();
    }
    public Task LoadStreaming()
    {
        while (true)
        {
            break;
        }
        return Task.CompletedTask;
    }
}
