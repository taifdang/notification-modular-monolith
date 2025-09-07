namespace UserProfile.UserPreferences.Dtos;
public record UserPreferenceDto(Guid Id, Guid UserId, List<PreferenceDto> Preferences);


