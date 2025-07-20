using MediatR;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage.CreateMessageAll;

public record CreateMessageAll : IRequest;

public class CreateMessageAllHandler : IRequestHandler<CreateMessageAll>
{
    public Task Handle(CreateMessageAll request, CancellationToken cancellationToken)
    {
        Console.WriteLine("receive message all");

        return Task.CompletedTask;
    }
}
