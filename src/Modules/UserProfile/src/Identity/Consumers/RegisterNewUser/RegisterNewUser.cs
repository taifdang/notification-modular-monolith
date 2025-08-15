using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserProfile.Data;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.Identity.Consumers.RegisterNewUser;

public class RegisterNewUser : IConsumer<UserCreated>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger<RegisterNewUser> _logger;

    public RegisterNewUser(UserProfileDbContext userProfileDbContext, IEventDispatcher eventDispatcher, 
        ILogger<RegisterNewUser> logger)
    {
        _userProfileDbContext = userProfileDbContext;
        _eventDispatcher = eventDispatcher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        _logger.LogInformation($"Consumer for {nameof(UserCreated)} started");

        var userProfileExist =
            await _userProfileDbContext.UserProfiles.AnyAsync(x => x.UserId.Value == context.Message.Id);

        if (userProfileExist)
            return;

        var userProfile = UserProfiles.Model.UserProfile.Create(UserProfileId.Of(NewId.NextGuid()),UserId.Of(context.Message.Id),
            UserName.Of(context.Message.UserName),Name.Of(context.Message.Name),Email.Of(context.Message.Email));

        await _userProfileDbContext.UserProfiles.AddAsync(userProfile);

        await _userProfileDbContext.SaveChangesAsync();

        await _eventDispatcher.SendAsync(
            new UserProfileCreatedDomainEvent(userProfile.Id,userProfile.UserId,userProfile.UserName,userProfile.Name,userProfile.Email),
            typeof(IInternalCommand));
    }
}
