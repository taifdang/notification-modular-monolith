using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Setting.Data;
using Setting.Data.Seeds;

namespace Setting.Identity.Consumers.RegisterNewUser;
public class RegisterNewUser : IConsumer<UserCreated>
{
    private readonly SettingDbContext _settingDbContext;
    public RegisterNewUser(SettingDbContext settingDbContext)
    {
        _settingDbContext = settingDbContext;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        Guard.Against.Null(context.Message, nameof(UserCreated));

        var settingExists = 
            await _settingDbContext.NotificationPreferences.AnyAsync(x => x.UserId == context.Message.Id);

        if (settingExists)
        {
            return;
        }

        var notificationPreference = NotificationPreferenceSeeder.CreateDefaultPreference(context.Message.Id);

        await _settingDbContext.NotificationPreferences.AddRangeAsync(notificationPreference);
        await _settingDbContext.SaveChangesAsync();
    }
}
