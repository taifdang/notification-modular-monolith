
using BuildingBlocks.Contracts;
using Notification.Application.Common.Interfaces;

namespace Notification.Infrastructure.Services;

public class RuleBaseService : IRuleBaseService
{
    public List<ChannelType> GetChannels(Domain.Entities.Notification notification, List<PreferenceDto> preferences)
    {
        return notification.NotificationType switch
        {
            NotificationType.Topup => new List<ChannelType> { ChannelType.InApp },
            NotificationType.ChangePassword => new List<ChannelType> { ChannelType.Email },
            _ => FilterRule(preferences)
        };
    }
    public List<ChannelType> FilterRule(List<PreferenceDto> preferences)
        => preferences.Where(p => !p.IsOptOut).Select(x => x.Channel).ToList();
}
