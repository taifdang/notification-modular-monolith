using BuildingBlocks.Contracts;
using BuildingBlocks.Core.Model;
using UserProfile.UserPreferences.Features.CompletingUserPreference;
using UserProfile.UserPreferences.ValueObject;

namespace UserProfile.UserPreferences.Model;
public record UserPreference : Aggregate<UserPreferenceId>
{
    public UserId UserId { get; private set; } = default!;
    //public Preference Preference { get; private set; } = default!;
    public ChannelType Channel { get; private set; }
    public bool IsOptOut { get; private set; }

    public static UserPreference Create(UserPreferenceId notificationSettingId, UserId userId, ChannelType channel,
        bool isOptOut, bool isDelete = false)
    {
        var notificationSetting = new UserPreference()
        {
            Id = notificationSettingId,
            UserId = userId,
            Channel = channel,
            IsOptOut = isOptOut,
            IsDeleted = isDelete,
        };

        return notificationSetting;
    } 

    public void CompletedRegisterNotificationSetting(UserPreferenceId notificationSettingId, UserId userId,
        ChannelType channel, bool isOptOut, bool isDelete = false)
    {
        this.Id = notificationSettingId;
        this.UserId = userId;
        this.Channel = channel;
        this.IsOptOut = isOptOut;
        this.IsDeleted = isDelete;

        var @event = new UserPreferenceRegistrationCompletedDomainEvent(this.Id, this.UserId, this.Channel, this.IsOptOut, this.IsDeleted);

        this.AddDomainEvent(@event);
    }

    public void UpdateOptOut(bool isOptOut)
    {
        if (this.IsOptOut != isOptOut)
        {
            this.IsOptOut = isOptOut;
            this.UpdatedAt = DateTime.UtcNow;

            //domain event
        }
    }
}
