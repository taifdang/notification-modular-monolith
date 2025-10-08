using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Data;
using User.Data.Seeder;
using User.Profiles.ValueObjects;

namespace User.Identity.Consumers.RegisteringNewUser;

public class RegisterNewUserHandler : IConsumer<UserCreated>
{
    private readonly UserDbContext _userDbContext;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger<RegisterNewUserHandler> _logger;

    public RegisterNewUserHandler(UserDbContext userDbContext, IEventDispatcher eventDispatcher, ILogger<RegisterNewUserHandler> logger)
    {
        _userDbContext = userDbContext;
        _eventDispatcher = eventDispatcher;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        _logger.LogInformation($"Consumer for {nameof(UserCreated)} started");

        var profileExist =
            await _userDbContext.Profiles.AnyAsync(x => x.UserId.Value == context.Message.Id);

        if (profileExist)
            return;

        var preferenceEntity = PreferenceSeeder.CreateDefaultPreference(context.Message.Id);

        var profileEntity = Profiles.Model.Profile.Create(ProfileId.Of(NewId.NextGuid()), UserId.Of(context.Message.Id),
            UserName.Of(context.Message.UserName), Name.Of(context.Message.Name), Email.Of(context.Message.Email));

        await _userDbContext.Profiles.AddAsync(profileEntity);

        await _userDbContext.Preferences.AddRangeAsync(preferenceEntity);

        await _userDbContext.SaveChangesAsync();

        //await _eventDispatcher.SendAsync(
        //    new UserCreatedDomainEvent(profileEntity.Id, profileEntity.UserId, profileEntity.UserName, profileEntity.Name, profileEntity.Email),
        //    typeof(IInternalCommand));
    }
}
