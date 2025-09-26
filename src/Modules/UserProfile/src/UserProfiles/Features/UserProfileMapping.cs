
using Mapster;
using UserProfile.UserProfiles.Dtos;
using UserProfile.UserProfiles.Features.CompletingRegisterUserProfile;

namespace UserProfile.UserProfiles.Features;

public class UserProfileMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        //config.NewConfig<CompleteRegisterUserProfileRequestDto, CompleteRegisterUserProfile>()
        //    .ConstructUsing(x => new CompleteRegisterUserProfile(x.UserId, x.GenderType, x.Age, x.Balance));

        config.NewConfig<UserProfiles.Model.UserProfile, UserProfileDto>()
            .ConstructUsing(x => new UserProfileDto(x.Id.Value, x.UserId.Value, x.Name.Value, x.GenderType, x.Age.Value));
    }
}
