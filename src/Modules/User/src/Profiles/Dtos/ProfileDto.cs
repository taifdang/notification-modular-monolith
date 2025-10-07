using User.Profiles.Enums;

namespace User.Profiles.Dtos;
public record ProfileDto(Guid Id, Guid UserId, string Name, GenderType GenderType, int Age);
