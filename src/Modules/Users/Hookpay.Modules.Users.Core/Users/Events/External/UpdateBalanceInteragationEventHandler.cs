using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Events.External;

public class UpdateBalanceInteragationEventHandler : IConsumer<TopupContracts>
{
    private readonly UserDbContext _context;
    public UpdateBalanceInteragationEventHandler(UserDbContext context) { _context = context; }
    public async Task Consume(ConsumeContext<TopupContracts> request)
    {
        //
        try
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.user_name == request.Message.username.ToLowerInvariant());
            if (user is null) throw new JobNotFoundException();
            user.user_balance += request.Message.tranferAmount;
            await _context.SaveChangesAsync();
            //push in message_tbl
            Console.WriteLine("[consumer]" + request.Message.username + "_" + request.Message.tranferAmount);
        }
        catch(DbUpdateConcurrencyException ex)
        {
            Console.WriteLine("[exception]>> " + ex.ToString());
        }

        
    }
}
