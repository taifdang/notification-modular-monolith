using BuildingBlocks.Core.Model;
using UserProfile.NotificationSettings.Features.CompletingNotificationSetting;
using UserProfile.NotificationSettings.ValueObject;

namespace UserProfile.NotificationSettings.Model;
public record NotificationSetting : Aggregate<NotificationSettingId>
{
    public UserId UserId { get; set; } = default!;
    public Preference Preference { get; private set; } = default!;

    public static NotificationSetting Create(NotificationSettingId notificationSettingId, UserId userId,
        Preference preference, bool isDelete = false)
    {
        var notificationSetting = new NotificationSetting()
        {
            Id = notificationSettingId,
            UserId = userId,
            Preference = preference,
            IsDeleted = isDelete
        };

        //create
        //var @event = new NotificationSettingRegistrationCompletedDomainEvent(notificationSetting.Id,notificationSetting.UserId,
        //    notificationSetting.Preference, notificationSetting.IsDeleted);

        //notificationSetting.AddDomainEvent(@event);

        return notificationSetting;
    } 

    public void CompletedRegisterNotificationSetting(NotificationSettingId notificationSettingId, UserId userId,
        Preference preference, bool isDelete = false)
    {
        this.Id = notificationSettingId;
        this.UserId = userId;
        this.Preference = preference;
        this.IsDeleted = isDelete;

        var @event = new NotificationSettingRegistrationCompletedDomainEvent(this.Id, this.UserId, this.Preference, this.IsDeleted);

        this.AddDomainEvent(@event);
    }
}
