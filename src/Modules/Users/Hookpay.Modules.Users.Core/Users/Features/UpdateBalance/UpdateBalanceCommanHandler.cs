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
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.userId);
            if (user is null) return null!;
            user.Balance += request.tranferAmount;
            await _context.SaveChangesAsync();
            //publish intergation event -> user
            return new UserDto { username = user.Username,email = user.Email,balance=user.Balance,phone=user.Phone};
        }
        catch
        {
            return null!;
        }
        
    }
}
