
using BuildingBlocks.Contracts;

namespace UserProfile.Data.Seeds;

public static class UserPreferenceSeeder
{
    public static List<UserPreferences.Model.UserPreference> CreateDefaultPreference(Guid UserId)
    {
        return Enum.GetValues<ChannelType>()
            .Select(channel => UserPreferences.Model.UserPreference.Create(
                UserPreferences.ValueObject.UserPreferenceId.Of(Guid.NewGuid()),
                UserPreferences.ValueObject.UserId.Of(UserId),
                channel,
                isOptOut: false))
            .ToList();
    }
}
