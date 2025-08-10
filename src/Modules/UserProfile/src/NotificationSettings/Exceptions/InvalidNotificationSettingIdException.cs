using BuildingBlocks.Exception;
using System.Net;

namespace UserProfile.NotificationSettings.Exceptions;
public class InvalidNotificationSettingIdException : AppException
{
    public InvalidNotificationSettingIdException(Guid Id) : base($"NotificationSettingId: {Id} is invalid")
    {
    }
}
