namespace UserProfile.GrpcServer.Services;

using Grpc.Core;
using Mapster;
using MediatR;
using System.Threading.Tasks;
using UserPreference;
using UserProfile.UserPreferences.Features.GettingUserPreferenceById;
using UserProfile.UserPreferences.Model;
using UserProfile.UserPreferences.ValueObject;
using GetUserPreferenceByIdResult = UserPreference.GetUserPreferenceByIdResult;
using GrpcChannelType = UserPreference.ChannelType;
public class UserPreferenceGrpcServices : UserPreferenceGrpcService.UserPreferenceGrpcServiceBase
{
    private readonly IMediator _mediator;

    public UserPreferenceGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetUserPreferenceByIdResult> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var result = new GetUserPreferenceByIdResult();

        var availabeEntity = await _mediator.Send(new GetUserPreferenceById(new Guid(request.Id)));

        if(availabeEntity?.UserPreferenceDto == null)
        {
            return result;
        }

        //map to grpc object
        //result.UserPreferenceDto.Id = availabeEntity.UserPreferenceDto.Id.ToString();
        //result.UserPreferenceDto.UserId = availabeEntity.UserPreferenceDto.UserId.ToString();

        //foreach (var pref in availabeEntity.UserPreferenceDto.Preferences)
        //{
        //    result.UserPreferenceDto.Preference.Add(availabeEntity.Adapt<UserPreferenceDto>());
        //}
        result.UserPreferenceDto = new UserPreferenceResponse
        {
            Id = availabeEntity.UserPreferenceDto.Id.ToString(),
            UserId = availabeEntity.UserPreferenceDto.UserId.ToString()
        };

        foreach (var pref in availabeEntity.UserPreferenceDto.Preferences)
        {
            result.UserPreferenceDto.Preference.Add(new UserDtoPreference
            {
                Channel = (GrpcChannelType)pref.Channel,
                IsOptOut = pref.IsOptOut
            });
        }

        //return result.Adapt<GetUserPreferenceByIdResult>();

        return result;
    }
}