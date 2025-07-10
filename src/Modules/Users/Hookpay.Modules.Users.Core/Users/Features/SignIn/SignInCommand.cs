using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.SignIn
{
    public record SignInCommand(string email,string password):IRequest<string>;
}
