using Hookpay.Shared.Contracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Events.External;

public class UpdateBalanceInteragationEventHandler : IConsumer<TopupContracts>
{
    public Task Consume(ConsumeContext<TopupContracts> context)
    {
        Console.WriteLine("[consumer]" +context.Message.username + "_" + context.Message.tranferAmount);
        return Task.CompletedTask;
    }
}
