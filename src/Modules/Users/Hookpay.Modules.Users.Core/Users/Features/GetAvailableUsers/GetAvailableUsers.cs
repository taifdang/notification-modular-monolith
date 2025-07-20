using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Dtos;
using Hookpay.Modules.Users.Core.Users.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Users.Core.Users.Features.GetAvailableUsers;

public record GetAvailableUsers : IRequest<GetAvailableUsersResult>;

public record GetAvailableUsersResult(IEnumerable<UserDto> UserDtos);

public record GetAvailableUsersHandler : IRequestHandler<GetAvailableUsers, GetAvailableUsersResult>
{
    private readonly UserDbContext _userDbContext;
    public GetAvailableUsersHandler(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }
    public async Task<GetAvailableUsersResult> Handle(GetAvailableUsers request, CancellationToken cancellationToken)
    {
        var user = 
            (
                await _userDbContext.Users.AsQueryable()
                .Where(x => 
                    !x.IsDeleted &&
                    x.Status == Enums.UserStatus.Active &&
                    x.UserSetting.AllowNotification == true)
                .Select(x=> new UserDto
                {
                    id = x.Id,
                    username = x.Username,
                    email = x.Email,
                    balance = x.Balance,
                    phone = x.Phone
                })
                .ToListAsync(cancellationToken)
            );

        if (!user.Any())
        {
            throw new Exception();
        }

        return new GetAvailableUsersResult(user);
    }
}
