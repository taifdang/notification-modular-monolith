using BuildingBlocks.Contracts;
using BuildingBlocks.Core.Model;
using Setting.NotificationPreferences.ValueObjects;

namespace Setting.NotificationPreferences.Model;
public record NotificationPreference : Aggregate<NotificationPreferenceId>
{
    public UserId UserId { get; private set; } = default!;
    public ChannelType Channel { get; private set; }
    public bool IsOptOut { get; private set; }
    public static NotificationPreference Create(NotificationPreferenceId notificationPreferenceId, UserId userId,
        ChannelType channel, bool isOptOut, bool isDelete = false)
    {
        var notificationPreference = new NotificationPreference()
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
