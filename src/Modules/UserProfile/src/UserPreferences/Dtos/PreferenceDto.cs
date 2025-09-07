using BuildingBlocks.Contracts;

namespace UserProfile.UserPreferences.Dtos;
public record PreferenceDto(ChannelType Channel, bool IsOptOut);
