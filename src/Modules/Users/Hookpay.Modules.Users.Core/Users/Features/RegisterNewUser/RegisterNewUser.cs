using Hookpay.Modules.Users.Core.Data;
using MediatR;

namespace Hookpay.Modules.Users.Core.Users.Features.RegisterNewUser
{
    public record RegisterNewUser(string username, string password, string email, string phone) : IRequest<RegisterNewUserResult>;

    public record RegisterNewUserResult(int userId, string username, string email, decimal balance, string phone);

    public class RegisterNewUserHandler : IRequestHandler<RegisterNewUser, RegisterNewUserResult>
    {
        private readonly UserDbContext _userDbContext;

        public RegisterNewUserHandler(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<RegisterNewUserResult> Handle(RegisterNewUser request, CancellationToken cancellationToken)
        {
            var user = new Models.Users
            {
                Username = request.username,
                Password = request.password,
                Email = request.email,
                Phone = request.phone,
            };

            await _userDbContext.Users.AddAsync(user);

            //note: processor after
            await _userDbContext.SaveChangesAsync();

            return new RegisterNewUserResult(user.Id, user.Username, user.Email, user.Balance, user.Phone);
        }
    }
}
