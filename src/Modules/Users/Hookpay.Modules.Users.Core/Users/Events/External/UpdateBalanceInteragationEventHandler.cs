using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Models;
using Hookpay.Shared.EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Events.External;

public class UpdateBalanceInteragationEventHandler : IConsumer<TopupContracts>
{
    private readonly UserDbContext _context;
    private readonly IBusPublisher _publisher;
    public UpdateBalanceInteragationEventHandler(UserDbContext context, IBusPublisher publisher) { _context = context; _publisher = publisher; }
    public async Task Consume(ConsumeContext<TopupContracts> request)
    {
        //
        try
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == request.Message.username.ToLowerInvariant());
            if (user is null) throw new Exception("Not found userId");
            user.Balance += request.Message.tranferAmount;
            await _context.SaveChangesAsync();
                       
            var payload = new MessagePayload
            {
                entity_id = request.Message.transId,
                event_type = "topup.created",
                action = PushType.InWeb,
                user_id = user.Id,
                detail = new Dictionary<string, object> {
                   {"entity_id",request.Message.transId },
                   {"user_id",user.Id},
                   {"transfer_amount",request.Message.tranferAmount }
                },
                priority = PriorityMessage.High
            };

            var messageContracts = new MessageContracts(Guid.NewGuid(),"topup.created", JsonSerializer.Serialize(payload));

            await _publisher.SendAsync<MessageContracts>(messageContracts);

            Console.WriteLine("[consumer]" + request.Message.username + "_" + request.Message.tranferAmount);
        }
        catch(DbUpdateConcurrencyException ex)
        {
            Console.WriteLine("[exception]>> " + ex.ToString());
        }

        
    }
}
