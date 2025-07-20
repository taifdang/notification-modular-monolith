using Hookpay.Modules.Notifications.Core.Messages.Models;
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
            int totalUser = 0;
            try
            {
                 totalUser = await _mediator.Send(new UserTotalItem());
            }
            catch
            {
                _logger.LogError("Occur error in load user total");
                throw;
            }
            int pageSize = 10;
            int pageCount = (int)Math.Ceiling(totalUser / (decimal)pageSize);
            //int pageCount = totalUser % pageSize > 0 ? totalUser / pageSize + 1 : totalUser / pageSize;
            int pageIndex = 0;

            while (pageIndex<pageCount)
            {
                var listUser = await _mediator.Send(new GetUserByPageIndex(pageIndex, pageSize));
                if (listUser == null || listUser.Count() ==0)
                {
                    _logger.LogError("listUser = null || 0");
                    break;
                }
                await SendInQueueAsync(listUser, message);
                pageIndex++;
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Occur error in hangfire caller:{ex}",ex);
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
            _logger.LogError("Occur error");
        }
    }
}
