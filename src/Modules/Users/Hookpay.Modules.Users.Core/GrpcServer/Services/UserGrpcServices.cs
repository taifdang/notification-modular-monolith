using Grpc.Core;
using Hookpay.Modules.Users.Core.Users.Features.GetAvailableUsers;
using MediatR;
using User;
using Mapster;
using Hookpay.Modules.Users.Core.Users.Features.GetAvailableUserById;



namespace Hookpay.Modules.Users.Core.GrpcServer.Services;

using GetAvailableUsersResult = User.GetAvailableUsersResult;

public class UserGrpcServices : UserGrpcService.UserGrpcServiceBase
{
    private readonly IMediator _mediator;

    public UserGrpcServices(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetAvailableUsersResult> GetAvailableUsers(GetAvailableUsersRequest request, ServerCallContext context)
    {
        var result = new GetAvailableUsersResult();

        var availableUsers = await _mediator.Send(
            new GetAvailableUsers((int)request.PageNumber, (int)request.PageSize));

        if (availableUsers?.UserDtos == null)
        {
            return result;
        }

        foreach (var availableUser in availableUsers.UserDtos)
        {
            result.UserDto.Add(availableUser.Adapt<UserDtoResponse>()); 
        }

        result.PageNumber = availableUsers.PageNumber;
        result.PageSize = availableUsers.PageSize;
        result.TotalPage = availableUsers.TotalPage;
        result.TotalItem = availableUsers.TotalItem;

        return result;
    }

    public override async Task<GetAvailaleUserByIdResult> GetAvailaleUserById(GetAvailaleUserByIdRequest request, ServerCallContext context)
    {
        var result = new GetAvailaleUserByIdResult();

        var availableUser = await _mediator.Send(
            new GetAvailableUserById((int)request.UserId));

        if (availableUser?.UserDto == null)
            return result;

        result.UserDto = availableUser.UserDto?.Adapt<UserDtoResponse>();

        return result;
    }
}
