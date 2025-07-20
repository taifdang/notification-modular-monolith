using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.GetUserByIds;

public record GetUserByIdsCommand(List<int> Ids):IRequest<List<int>>;

