namespace Notification.Configurations.Rules;

using BuildingBlocks.Contracts;
using Notification.Notifications.Model;
public class NotificationRule
{
    //Rule-based engine to get channel base on notification type and user preference
    public static List<ChannelType> GetChannels(Notification notification, List<PreferenceDto> preferences)
    {
        //default channel and list channel base on rule
        return notification.NotificationType switch
        {
            NotificationType.Topup => new List<ChannelType> { ChannelType.InApp },
            NotificationType.ChangePassword => new List<ChannelType> { ChannelType.Email },
            _ => FilterRule(preferences)
        };
    }
    public static List<ChannelType> FilterRule(List<PreferenceDto> preferences)
        => preferences.Where(p => !p.IsOptOut).Select(x => x.Channel).ToList();

}
