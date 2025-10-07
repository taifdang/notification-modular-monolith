namespace User.Preferences.Model;

using BuildingBlocks.Contracts;
using BuildingBlocks.Core.Model;
using User.Preferences.ValueObjects;

public record Preference : Aggregate<PreferenceId>
{
    public UserId UserId { get; private set; } = default!;
    public ChannelType Channel { get; private set; }
    public bool IsOptOut { get; private set; }

    public static Preference Create(PreferenceId notificationPreferenceId, UserId userId,
       ChannelType channel, bool isOptOut, bool isDelete = false)
    {
        var notificationPreference = new Preference()
        {
            Id = notificationPreferenceId,
            UserId = userId,
            Channel = channel,
            IsOptOut = isOptOut,
            IsDeleted = isDelete,
        };
        return notificationPreference;
    }
    public void UpdateOptOut(bool isOptOut)
    {
        if (this.IsOptOut != isOptOut)
        {
            this.IsOptOut = isOptOut;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
