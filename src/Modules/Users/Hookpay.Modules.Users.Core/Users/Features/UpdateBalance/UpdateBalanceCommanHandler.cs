using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Dao;
using Hookpay.Modules.Users.Core.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.UpdateBalance;

public class UpdateBalanceCommanHandler : IRequestHandler<UpdateBalanceCommand, UserDto>
{
    private readonly UserDbContext _context;
    public UpdateBalanceCommanHandler(UserDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.user_id == request.userId);
            if (user is null) return null!;
            user.user_balance += request.tranferAmount;
            await _context.SaveChangesAsync();
            return new UserDto { username = user.user_name,email = user.user_email,balance=user.user_balance,phone=user.user_phone};
        }
        catch
        {
            return null!;
        }
        
    }
}
