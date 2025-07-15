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
        var list_userId = await _context.Users
            .Where(x => request.Ids.Contains(x.Id))
            .Join(
                _context.UserSetting,
                user => user.Id,
                userSetting => userSetting.UserId,
                (user, userSetting) => new { user, userSetting })
            .Where(x =>
                x.userSetting.AllowNotification == false &&
                x.user.Status == UserStatus.Active)
            .Select(x => x.user.Id)
            .ToListAsync();
        return list_userId;
    }
}
