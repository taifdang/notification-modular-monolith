using BuildingBlocks.Core.Model;
using Profile.Profiles.Enums;
using Profile.Profiles.ValueObjects;

namespace Profile.Profiles.Model;
public record Profile : Aggregate<ProfileId>
{
    public UserId UserId { get; private set; } = default!;
    public UserName UserName { get; private set; } = default!;//option*
    public Name Name { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public GenderType GenderType { get; private set; }
    public Age? Age { get; private set; }
}
