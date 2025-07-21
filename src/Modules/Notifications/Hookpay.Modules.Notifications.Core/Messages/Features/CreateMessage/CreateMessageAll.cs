using MediatR;
using Microsoft.Extensions.Logging;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public record CreateMessageAll(string message) : IRequest;

public class CreateMessageAllHandler : IRequestHandler<CreateMessageAll>
{
    private readonly ICreateMessageProcessor _processor;
    private readonly ILogger<CreateMessageAllHandler> _logger;
    public CreateMessageAllHandler(
        ICreateMessageProcessor processor,
        ILogger<CreateMessageAllHandler> logger)
    {
        _processor = processor;
        _logger = logger;
    }
    public async Task Handle(CreateMessageAll request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"schedule job processor message all starting ...");

        await _processor.AddAllMessageAsync(request.message);         
    }
}
