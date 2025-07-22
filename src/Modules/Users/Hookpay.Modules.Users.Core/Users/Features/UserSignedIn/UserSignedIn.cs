using Hookpay.Modules.Users.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Hookpay.Shared.Jwt;
using Hookpay.Shared.Exceptions;

namespace Hookpay.Modules.Users.Core.Users.Features.UserSignedIn;

public record UserSignedIn(string email, string password) : IRequest<UserSignedInResult>;

public record UserSignedInResult(string accessToken, string? refreshToken);

public class UserSignedInHandler : IRequestHandler<UserSignedIn, UserSignedInResult>
{
    private readonly UserDbContext _userDbContext;
    public UserSignedInHandler(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }
    public async Task<UserSignedInResult> Handle(UserSignedIn request, CancellationToken cancellationToken)
    {
        var user = (await _userDbContext.Users.SingleOrDefaultAsync(x => x.Email == request.email));

        if (user is null)
        {
            throw new NotFoundException("Not found user");
        }

        if (!user.Password.Equals(request.password))
        {
            throw new Exception("Not match password");
        }

        var accessToken = JwtGenerate.GenerateJwtToken(user.Id, user.Email, user.Username);

        //exception

        return new UserSignedInResult(accessToken,"");
    }
}
