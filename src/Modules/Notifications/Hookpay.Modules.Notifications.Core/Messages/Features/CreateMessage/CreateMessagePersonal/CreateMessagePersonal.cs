using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage.CreateMessagePersonal;

public record CreateMessagePersonal : IRequest;

public class CreateMessagePersonalHandler : IRequestHandler<CreateMessagePersonal>
{
    public Task Handle(CreateMessagePersonal request, CancellationToken cancellationToken)
    {
        Console.WriteLine("recei message all");

        return Task.CompletedTask;
    }
}
