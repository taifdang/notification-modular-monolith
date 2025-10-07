namespace User.Preferences.Dtos;
public record PreferenceDto(Guid Id, Guid UserId, List<ChannelPreference> Preferences);

