using Hookpay.Modules.Users.Core.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.UpdateBalance;

public record UpdateBalanceCommand(int userId,decimal tranferAmount):IRequest<UserDto>;

