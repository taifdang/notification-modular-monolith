
using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Dtos;
using Hookpay.Modules.Users.Core.Users.Exceptions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Users.Core.Users.Features.GetAvailableUserById;

public record GetAvailableUserById(int UserId) : IRequest<GetAvailabeUserByIdResult>;

public record GetAvailabeUserByIdResult(UserDto UserDto);

public class GetAvailableUserByIdHandler : IRequestHandler<GetAvailableUserById, GetAvailabeUserByIdResult>
{
    private readonly UserDbContext _userDbContext;
    private readonly IMapper _mapper;

    public GetAvailableUserByIdHandler(
        UserDbContext userDbContext,
        IMapper mapper)
    {
        _userDbContext = userDbContext;
        _mapper = mapper;
    }

    public async Task<GetAvailabeUserByIdResult> Handle(GetAvailableUserById request, CancellationToken cancellationToken)
    {
        var user = await _userDbContext.Users
            .FirstOrDefaultAsync(x => 
                x.Id == request.UserId &&
                !x.IsDeleted,
                cancellationToken);

        if (user is null)
            throw new UserAlreadyExistException();

        var userDto = _mapper.Map<UserDto>(user);

        return new GetAvailabeUserByIdResult(userDto);
    }
}
