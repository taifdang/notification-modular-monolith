using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Events.External;

public class GetTotalUserHandler : IRequestHandler<UserTotalItem, int>
{
    private readonly UserDbContext _context;
    public GetTotalUserHandler(UserDbContext context)
    {
        _context = context;
    }
    public   Task<int> Handle(UserTotalItem request, CancellationToken cancellationToken)
    {
        var total = _context.Users.Select(x=>x.Id).Count();
        
        return Task.FromResult(total);
    }
}
