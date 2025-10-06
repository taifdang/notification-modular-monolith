using BuildingBlocks.Contracts;

namespace Setting.Data.Seeds;
public static class NotificationPreferenceSeeder
{
    public static List<NotificationPreferences.Model.NotificationPreference> CreateDefaultPreference(Guid UserId)
    {
        return Enum.GetValues<ChannelType>()
            .Select(channel => NotificationPreferences.Model.NotificationPreference.Create(
                NotificationPreferences.ValueObjects.NotificationPreferenceId.Of(Guid.NewGuid()),
                NotificationPreferences.ValueObjects.UserId.Of(UserId),
                channel,
                isOptOut: false))
            .ToList();
    }
}
