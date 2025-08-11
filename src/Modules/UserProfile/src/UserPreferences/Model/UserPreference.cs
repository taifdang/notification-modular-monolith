using BuildingBlocks.Core.Model;
using UserProfile.UserPreferences.Features.CompletingUserPreference;
using UserProfile.UserPreferences.ValueObject;

namespace UserProfile.UserPreferences.Model;
public record UserPreference : Aggregate<UserPreferenceId>
{
    public UserId UserId { get; set; } = default!;
    public Preference Preference { get; private set; } = default!;

    public static UserPreference Create(UserPreferenceId notificationSettingId, UserId userId,
        Preference preference, bool isDelete = false)
    {
        var notificationSetting = new UserPreference()
        {
            Id = notificationSettingId,
            UserId = userId,
            Preference = preference,
            IsDeleted = isDelete
        };

        //create
        //var @event = new UserPreferenceRegistrationCompletedDomainEvent(notificationSetting.Id,notificationSetting.UserId,
        //    notificationSetting.Preference, notificationSetting.IsDeleted);

        //notificationSetting.AddDomainEvent(@event);

        return notificationSetting;
    } 

    public void CompletedRegisterNotificationSetting(UserPreferenceId notificationSettingId, UserId userId,
        Preference preference, bool isDelete = false)
    {
        this.Id = notificationSettingId;
        this.UserId = userId;
        this.Preference = preference;
        this.IsDeleted = isDelete;

        var @event = new UserPreferenceRegistrationCompletedDomainEvent(this.Id, this.UserId, this.Preference, this.IsDeleted);

        this.AddDomainEvent(@event);
    }
}
