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

public class GetUserByPageIndexHandler : IRequestHandler<GetUserByPageIndex, List<int>>
{
    private readonly UserDbContext _context;
    public GetUserByPageIndexHandler(UserDbContext context)
    {
        _context = context;
    }
    public async Task<List<int>> Handle(GetUserByPageIndex request, CancellationToken cancellationToken)
    {
        return await _context.Users.
             OrderBy(x=>x.Id)
            .Skip(request.pageSize*request.pageIndex)
            .Take(request.pageSize)
            .Select(x => x.Id)
            .ToListAsync();
    }
}
