using Hookpay.Modules.Users.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, string>
{
    private readonly UserDbContext _context;
    public SignInCommandHandler(UserDbContext context) {  _context = context; }
    public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        //email:index non-cluster
        var user =  await _context.users.SingleOrDefaultAsync(x => x.user_email == request.email);
        if (user is null) return string.Empty;
        //var result = GenerateToken();
        throw new NotImplementedException();
    }
    public string GenerateToken(string email,string username)
    {
        throw new NotImplementedException();

    }
}
