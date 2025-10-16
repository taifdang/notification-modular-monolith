
using BuildingBlocks.Contracts;

namespace Notification.Application.Common.Interfaces;

public interface IRuleBaseService
{
    List<ChannelType> GetChannels(Domain.Entities.Notification notification, List<PreferenceDto> preferences);
    List<ChannelType> FilterRule(List<PreferenceDto> preferences);
}
