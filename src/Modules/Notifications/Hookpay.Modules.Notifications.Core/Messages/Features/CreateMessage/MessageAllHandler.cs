using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.EventBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public class MessageAllHandler
{
    private readonly ILogger<MessageAllHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IServiceScopeFactory _provider;
    public MessageAllHandler(ILogger<MessageAllHandler> logger, IMediator mediator, IServiceScopeFactory provider)
    {
        _logger = logger;
        _mediator = mediator;
        _provider = provider;
    }
    public async Task LoadDataStreaming(Message message)
    {
        try
        {
            int totalUser = await _mediator.Send(new UserTotalItem());
            int pageSize = 10;
            int pageCount = (int)Math.Ceiling(totalUser / (decimal)pageSize);
            int pageIndex = 0;

            while (true)
            {
                var listUser = await _mediator.Send(new GetUserByPageIndex(pageIndex, pageSize));
                if (listUser == null) break;
                await SendInQueueAsync(listUser, message);
                pageIndex++;
            }
        }
        catch
        {
            throw new Exception("Occur error in hangfire caller");
        }
    }   
    public async Task SendInQueueAsync(List<int> UserIds, Message message)
    {
        using var scope = _provider.CreateScope();
        var _publisher = scope.ServiceProvider.GetRequiredService<IBusPublisher>();
        try
        {
            foreach (var id in UserIds)
            {
                await _publisher.SendAsync<MessageAllContracts>(new MessageAllContracts(message.CorrelationId, id, message.Title, message.Body));
            }
        }
        catch
        {
            throw new Exception("Occur error");
        }
    }
}
