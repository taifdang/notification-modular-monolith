using MediatR;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public record CreateMessagePersonal : IRequest;

public class CreateMessagePersonalHandler : IRequestHandler<CreateMessagePersonal>
{
    public Task Handle(CreateMessagePersonal request, CancellationToken cancellationToken)
    {
        Console.WriteLine("receive message personal");

        return Task.CompletedTask;
    }
}
