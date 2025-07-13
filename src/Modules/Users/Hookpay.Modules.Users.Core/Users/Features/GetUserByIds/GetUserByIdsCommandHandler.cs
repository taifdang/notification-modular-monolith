using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Models;
using Hookpay.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.GetUserByIds;

public class GetUserByIdsCommandHandler : IRequestHandler<UserFlilterContracts, List<int>>
{
    private readonly UserDbContext _context;
    public GetUserByIdsCommandHandler(UserDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> Handle(UserFlilterContracts request, CancellationToken cancellationToken)
    {
        var list_userId = await _context.users
            .Where(x => request.Ids.Contains(x.user_id))
            .Join(
                _context.settings,
                user => user.user_id,
                userSetting => userSetting.set_user_id,
                (user, userSetting) => new { user, userSetting })
            .Where(x =>
                x.userSetting.disable_notification == false &&
                x.user.is_block == false)
            .Select(x => x.user.user_id)
            .ToListAsync();
        return list_userId;
    }
}
