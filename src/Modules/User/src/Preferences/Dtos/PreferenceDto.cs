namespace User.Preferences.Dtos;
public record PreferenceDto(Guid UserId, IEnumerable<ChannelPreference> Preferences);