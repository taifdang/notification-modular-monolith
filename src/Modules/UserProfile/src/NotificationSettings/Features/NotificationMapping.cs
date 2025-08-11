using Mapster;
using UserProfile.NotificationSettings.Dtos;
using UserProfile.NotificationSettings.Features.CompletingNotificationSetting;
using UserProfile.NotificationSettings.Model;
using UserProfile.NotificationSettings.ValueObject;

namespace UserProfile.NotificationSettings.Features;

//ref: https://code-maze.com/mapster-aspnetcore-introduction/
public class NotificationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CompleteNotificationSettingResquestDto, CompletedNotificationSetting>()
            .ConstructUsing(x => new CompletedNotificationSetting(x.UserId, x.Preference));

        config.NewConfig<NotificationSetting, NotificationSettingDto>()
            .ConstructUsing(x => new NotificationSettingDto(x.Id, x.UserId, x.Preference));

        config.NewConfig<CompletedNotificationSettingMonoCommand, NotificationSetting>()
             .Map(d => d.Id, s => NotificationSettingId.Of(s.Id))
                .Map(d => d.UserId, s => UserId.Of(s.UserId))
                    .Map(d => d.Preference, s => Preference.Of(s.Preference));

    }
}
