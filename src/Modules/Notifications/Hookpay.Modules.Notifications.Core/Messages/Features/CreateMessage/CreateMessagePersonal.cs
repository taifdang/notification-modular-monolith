using MediatR;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public record CreateMessagePersonal(int UserId, string Message) : IRequest;

public class CreateMessagePersonalHandler : IRequestHandler<CreateMessagePersonal>
{
    private readonly ICreateMessageProcessor _processor;
    
    public CreateMessagePersonalHandler(ICreateMessageProcessor processor)
    {
        _processor = processor;
    }
    public async Task Handle(CreateMessagePersonal request, CancellationToken cancellationToken)
    {
        Console.WriteLine("receive message personal");

        await _processor.AddPersonalMessageAsync(request.UserId, request.Message);
    }
}
