using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfile.Data;
using UserProfile.UserProfiles.Exceptions;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.UserProfiles.Features.UpdatingUserBalance;

public record UpdateUserBalance(int TopupId, string UserName, decimal TransferAmount) : InternalCommand;

public class UpdateUserBalanceHandler : ICommandHandler<UpdateUserBalance>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateUserBalanceHandler(
        UserProfileDbContext userProfileDbContext,
        IPublishEndpoint publishEndpoint,
        IEventDispatcher eventDispatcher)
    {
        _userProfileDbContext = userProfileDbContext;
        _publishEndpoint = publishEndpoint;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<Unit> Handle(UpdateUserBalance request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var userProfile = await _userProfileDbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.UserName.Value == request.UserName);

        if (userProfile is null) 
        { 
            throw new UserProfileNotExist(); 
        }

        userProfile.Update(userProfile.Id, userProfile.UserId, userProfile.UserName, userProfile.Name, userProfile.Email,
            userProfile.GenderType, userProfile.Age, Balance.Of(userProfile.Balance.Value + request.TransferAmount));

        _userProfileDbContext.UserProfiles.Update(userProfile);

        await _userProfileDbContext.SaveChangesAsync();

        //signal set new state (state machine)
        await _publishEndpoint.Publish(
            new BalanceUpdated(request.TopupId, userProfile.UserId, request.TransferAmount),
            cancellationToken);
        
        //send integration event in notification module
        var @event = new PersonalNotificationRequested(
            NotificationType.Topup,
            new Recipient(userProfile.UserId, userProfile.Email),
            DictionaryExtensions.SetPayloads
            (
                ("TopupId", request.TopupId),
                ("TransferAmount", request.TransferAmount)
            ),
            NotificationPriority.High);

        await _eventDispatcher.SendAsync(@event, cancellationToken: cancellationToken);

        return Unit.Value;          
    }
}

