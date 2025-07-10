using Hookpay.Modules.Users.Core.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.SignUp;

public record CreateUserCommand(string username,string password,string? email,string? phone):IRequest<UserDto>;
