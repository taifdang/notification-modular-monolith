using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.Caching;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
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

    public async Task AddAllMessageAsync(
        string data,
        CancellationToken cancellationToken = default)
    {
        int pageNumber = 1;
        int totalPage = 1;
        int pageSize = 10;

        try
        {
            while (true)
            {
                if (pageNumber > totalPage)
                {
                    break;
                }

                var users = _userGrpcServiceClient.GetAvailableUsers(
                    new GetAvailableUsersRequest 
                    { 
                        PageNumber = pageNumber, 
                        PageSize = pageSize 
                    },
                    cancellationToken: cancellationToken);

                if (users is null)
                {
                    throw new Exception("User is not empty");
                }

                await ProcessAllAsync(
                    users.UserDto,
                    data
                    );

                totalPage = users.TotalPage;
                pageNumber++;
            }
        }
        catch
        {
            throw new Exception("User loop is not fail");
        }
    }

    public async Task AddPersonalMessageAsync(
        int userId,
        string data,     
        CancellationToken cancellationToken = default)
    {
        if (userId < 0 || string.IsNullOrWhiteSpace(data))
            return;

        var alert = JsonSerializer.Deserialize<Alert>(data);

        if (userId != alert.UserId)
            return;

        var user = await IsExistUser(userId);   

        if(user is not null)
        {
            await ProcessAsync(userId, data);
        } 
    }

  
    public async Task ProcessAllAsync<T>(
        IReadOnlyList<T> users, 
        string data,
        CancellationToken cancellationToken = default)
    {
        try
        {
            //ref: https://dotnettutorials.net/lesson/dynamic-type-in-csharp/
            foreach (dynamic? user in users)
            {
                await ProcessAsync(
                    user!.Id, 
                    data
                    );
            }
        }
        catch
        {
            throw new Exception("List data is invalid");
        }
    }

    public async Task ProcessAsync(
        int userId,
        string data,
        CancellationToken cancellationToken = default)
    {
        //Deserialize
        var alert = JsonSerializer.Deserialize<Alert>(data);

        if (alert is null)
        {
            throw new Exception("alert fail");
        }
        
        //Push in queue => consumer push allow pushType
         await _publisher.Publish(
            new MessageEvent(userId, alert), 
            cancellationToken);

        //await SaveStatePublishMessage();

    }

    public Task SaveStatePublishMessage(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<GetAvailaleUserByIdResult> IsExistUser(int UserId, CancellationToken cancellationToken = default)
    {

        var user = _userGrpcServiceClient.GetAvailaleUserById(
            new GetAvailaleUserByIdRequest { UserId = UserId},
            cancellationToken: cancellationToken);

        if(user is null)
            return null;
        return user;
    }
}
