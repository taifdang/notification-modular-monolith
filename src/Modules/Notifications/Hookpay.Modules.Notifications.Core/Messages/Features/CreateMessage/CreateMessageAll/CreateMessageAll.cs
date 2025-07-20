using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage.CreateMessageAll;

public record CreateMessageAll : IRequest;

public class CreateMessageAllHandler : IRequestHandler<CreateMessageAll>
{
    public Task Handle(CreateMessageAll request, CancellationToken cancellationToken)
    {
        Console.WriteLine("recei message personal");

        return Task.CompletedTask;
    }
}
