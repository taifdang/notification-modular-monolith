namespace Notification.Configurations.Rules;

using BuildingBlocks.Constants;
using BuildingBlocks.Contracts;
using Notification.Notifications.Model;
public class NotificationRule
{
    public static List<string> SelectChannels(Notification notification, 
        NotificationConstant.Preferences userPreferenre)
    {
        return notification.NotificationType switch
        {
            NotificationType.Topup => new List<string> { nameof( ChannelType.InApp ) },
            NotificationType.ChangePassword => new List<string> { nameof(ChannelType.Email) },
            _ => SelectUserPreferenceOptout(notification, userPreferenre)
        };
    }
    internal static List<string> SelectUserPreferenceOptout(Notification notification,
        NotificationConstant.Preferences userPreferenre)
    {
        var channels = new List<string>();

        var optOut = userPreferenre.optOut;

        try
        {
            //ref:https://www.codepractice.io/convert-object-to-list-in-csharp
            foreach (var item in optOut.GetType().GetProperties())
            {
                if ((bool)item.GetValue(optOut)!)
                {
                    channels.Add(item.Name);
                }
            }
        }
        catch (Exception ex) 
        {
            throw new Exception("Occur error when selete user preference opt-out",ex);
        } 
        return channels;
    }
}
