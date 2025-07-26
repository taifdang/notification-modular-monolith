using Hookpay.Modules.Notifications.Core.Messages.Enums;
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
    public async Task AddAllMessageAsync(
        string message, 
        PushType pushType, 
        CancellationToken cancellationToken = default)
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

                await ProcessAllAsync(users.UserDto, message);

                TotalPage = users.TotalPage;
                PageNumber++;
            }
        }
        catch
        {
            throw new Exception("User loop is not fail");
        }
    }

    public async Task AddPersonalMessageAsync(
        int userId,
        string message,
        PushType pushType,
        CancellationToken cancellationToken = default)
    {
        if (userId < 0 || string.IsNullOrWhiteSpace(message))
            return;

        var user = await IsExistUser(userId);   

        if(user is not null)
        {
            await PublishAsync(userId, message, pushType);
        } 
    }


    public async Task MessageLoadingProcessor(
        string? message,
        MessageProcessorType processorType, 
        CancellationToken cancellationToken = default)
    {
        throw new Exception("User loop is not fail");
    }
  
    public async Task ProcessAllAsync<T>(IReadOnlyList<T> listUser, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            //ref: https://dotnettutorials.net/lesson/dynamic-type-in-csharp/
            foreach (dynamic? user in listUser)
            {
                await PublishAsync(user!.Id, message);
            }
        }
        catch
        {
            throw new Exception("List data is invalid");
        }
    }

    public async Task PublishAsync(
        int userId, 
        string message,
        PushType PushType, 
        CancellationToken cancellationToken = default)
    {
        //Push in queue => consumer push allow pushType

        await _publisher.Publish( , cancellationToken);

        await SaveStatePublishMessage();

        Console.WriteLine($"userId: {userId} :: message: {message}");

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
