using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Utils;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using UserProfile.Data;
using UserProfile.UserProfiles.Exceptions;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.Topups.Consumers.CreatingTopup;
public class CreateTopup : IConsumer<TopupCreated>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger<CreateTopup> _logger;

    public CreateTopup(UserProfileDbContext userProfileDbContext, IEventDispatcher eventDispatcher, ILogger<CreateTopup> logger)
    {
        _userProfileDbContext = userProfileDbContext;
        _eventDispatcher = eventDispatcher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TopupCreated> context)
    {
        _logger.LogInformation($"Consumer of {nameof(TopupCreated)} is started");

        if (context.Message is null)
            return;

        var user = await _userProfileDbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserName.Value == context.Message.username);
        if(user is null)
        {
            throw new UserProfileNotExist();
        }

        user.Update(user.Id,user.UserId,user.UserName,user.Name,user.Email,user.GenderType,user.Age, 
            Balance.Of(user.Balance.Value + context.Message.transferAmount));

        _userProfileDbContext.UserProfiles.Update(user);
        await _userProfileDbContext.SaveChangesAsync();

        //ref: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
        var @event = new PersonalNotificationCreated(
            NotificationType.Topup,
            new Recipient(user.Id,user.Email),
            DictionaryExtensions.SetPayloads
            (
                ("TopupId", context.Message.id),
                ("TransferAmount", context.Message.transferAmount)
            ),
            NotificationPriority.High);

        await _eventDispatcher.SendAsync(@event);
    }
}
