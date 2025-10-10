using Grpc.Core;
using MediatR;
using User.Preferences.Features.GettingPreferenceById;

namespace User.GrpcServer.Services;
public class UserGrpcServices : UserGrpcService.UserGrpcServiceBase
{
    private readonly IMediator _mediator;

    public UserGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetPreferenceByIdResult> GetPreferenceById(GetPreferenceByIdRequest request, ServerCallContext context)
    {
        var result = new GetPreferenceByIdResult();

        var preference = await _mediator.Send(new GetPreferenceById(new Guid(request.Id)));

        if (preference?.PreferenceDto == null)
        {
            return result;
        }

        result.PreferenceDto = new PreferenceResponse
        {
            UserId = preference.PreferenceDto.UserId.ToString()
        };

        foreach (var pref in preference.PreferenceDto.Preferences)
        {
            result.PreferenceDto.Preference.Add(new ChannelPreference
            {
                Channel = (User.ChannelType)pref.Channel,
                IsOptOut = pref.IsOptOut
            });
        }

        return result;
    }
}
