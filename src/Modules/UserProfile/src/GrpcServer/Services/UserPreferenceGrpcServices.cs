namespace UserProfile.GrpcServer.Services;

using Grpc.Core;
using Mapster;
using MediatR;
using System.Threading.Tasks;
using UserPreference;
using UserProfile.UserPreferences.Features.GettingUserPreferenceById;
using GetUserPreferenceByIdResult = UserPreference.GetUserPreferenceByIdResult;
public class UserPreferenceGrpcServices : UserPreferenceGrpcService.UserPreferenceGrpcServiceBase
{
    private readonly IMediator _mediator;

    public UserPreferenceGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetUserPreferenceByIdResult> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetUserPreferenceById(new Guid(request.Id)));
        return result.Adapt<GetUserPreferenceByIdResult>();
    }
}