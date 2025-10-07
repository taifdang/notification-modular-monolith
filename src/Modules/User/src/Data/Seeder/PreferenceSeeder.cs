namespace User.Data.Seeder;

using BuildingBlocks.Contracts;
using MassTransit;

public static class PreferenceSeeder
{
    public static List<Preferences.Model.Preference> CreateDefaultPreference(Guid UserId)
    {
        return Enum.GetValues<ChannelType>()
            .Select(channel => Preferences.Model.Preference.Create(
                Preferences.ValueObjects.PreferenceId.Of(NewId.NextGuid()),
                Preferences.ValueObjects.UserId.Of(UserId),
                channel,
                isOptOut: false))
            .ToList();
    }
}
