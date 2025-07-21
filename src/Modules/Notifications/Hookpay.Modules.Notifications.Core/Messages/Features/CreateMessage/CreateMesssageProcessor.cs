using Hookpay.Shared.Caching;
using MassTransit;
using MediatR;
using User;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
public class CreateMesssageProcessor : ICreateMessageProcessor
{
    private readonly IPublishEndpoint _publisher;
    private readonly IMediator _mediator;
    private readonly UserGrpcService.UserGrpcServiceClient _userGrpcServiceClient;
    private readonly IRequestCache _requestCache;
    public CreateMesssageProcessor(
        IPublishEndpoint publisher, 
        IMediator mediator,
        UserGrpcService.UserGrpcServiceClient userGrpcServiceClient,
        IRequestCache requestCache)
    {
        _publisher = publisher;
        _mediator = mediator;
        _userGrpcServiceClient = userGrpcServiceClient;
        _requestCache = requestCache;
    }
    public async Task AddAllMessageAsync(string message,CancellationToken cancellationToken = default)
    {       
        int PageNumber = 1;
        int TotalPage = 1;
        int PageSize = 10;
        
        try
        {
            while (true)
            {
                if (PageNumber > TotalPage)
                {
                    break;
                }

                var users = _userGrpcServiceClient.GetAvailableUsers(
                    new GetAvailableUsersRequest { PageNumber = PageNumber, PageSize = PageSize },
                    cancellationToken: cancellationToken);

                if (users is null)
                {
                    throw new Exception("User is not empty");
                }
                           
                //signalR
                foreach (var user in users.UserDto)
                {
                    await PublishAsync((int)user.Id, message);
                }

                TotalPage = users.TotalPage;
                PageNumber++;
            }
        }
        catch
        {
            throw new Exception("User loop is not fail");
        }


    }

    public Task AddPersonalMessageAsync(CancellationToken cancellationToken = default)
    {      
        throw new NotImplementedException();
    }

    public Task LoadCacheDataAsync(CancellationToken cancellationToken = default)
    {
        //null
        var users = _userGrpcServiceClient.GetAvailableUsers(
            new GetAvailableUsersRequest { PageNumber = 1, PageSize = 10},
            cancellationToken: cancellationToken);
        throw new NotImplementedException();
    }

    public Task PublishAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task PublishAsync(int userId, string message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"userId: {userId} :: message: {message}");

        return Task.CompletedTask;  
    }
}
