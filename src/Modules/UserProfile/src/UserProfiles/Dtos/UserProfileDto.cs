using UserProfile.UserProfiles.Enums;

namespace UserProfile.UserProfiles.Dtos;

public record UserProfileDto(Guid Id, Guid UserId, string Name, GenderType GenderType, int Age);
