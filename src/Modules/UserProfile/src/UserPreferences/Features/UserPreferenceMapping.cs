using Mapster;
using UserProfile.UserPreferences.Dtos;
using UserProfile.UserPreferences.Features.CompletingUserPreference;
using UserProfile.UserPreferences.Model;
using UserProfile.UserPreferences.ValueObject;

namespace UserProfile.UserPreferences.Features;

//ref: https://code-maze.com/mapster-aspnetcore-introduction/
public class UserPreferenceMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CompleteUserPreferenceResquestDto, CompletedUserPreference>()
            .ConstructUsing(x => new CompletedUserPreference(x.UserId, x.Preference));

        config.NewConfig<UserPreference, UserPreferenceDto>()
            .ConstructUsing(x => new UserPreferenceDto(x.Id, x.UserId, x.Preference));

        /* class valueobject
        config.NewConfig<CompletedUserPreferenceMonoCommand, UserPreference>()
              .Map(d => d.Id, s => UserPreferenceId.Of(s.Id))
                .Map(d => d.UserId, s => UserId.Of(s.UserId))
                    .Map(d => d.Preference, s => Preference.Of(s.Preference));
        */
        TypeAdapterConfig<Guid, UserPreferenceId>.NewConfig()
            .MapWith(src => UserPreferenceId.Of(src));

        TypeAdapterConfig<Guid, UserId>.NewConfig()
            .MapWith(src => UserId.Of(src));

        TypeAdapterConfig<string, Preference>.NewConfig()
            .MapWith(src => Preference.Of(src));

        config.NewConfig<CompletedUserPreferenceMonoCommand, UserPreference>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.UserId, s => s.UserId)
            .Map(d => d.Preference, s => s.Preference);
    }
}
