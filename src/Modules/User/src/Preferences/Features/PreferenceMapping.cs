using Mapster;
namespace User.Preferences.Features;
public class PreferenceMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Model.Preference, Dtos.ChannelPreference>.NewConfig()
            .Map(dest => dest.Channel, src => src.Channel)
            .Map(dest => dest.IsOptOut, src => src.IsOptOut);

        TypeAdapterConfig<List<Model.Preference>, Dtos.PreferenceDto>.NewConfig()
            .Map(dest => dest.Id, src => src.First().Id)
            .Map(dest => dest.UserId, src => src.First().UserId)
            .Map(dest => dest.Preferences, src => src.Adapt<List<ChannelPreference>>());
    }
}
