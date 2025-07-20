using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Models;
using Hookpay.Shared.EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hookpay.Modules.Users.Core.Topups.Consumers
{
    public class Message() {
        public int entity_id { get; set; }
        public PushType action { get; set; } = default;
        public string event_type { get; set; } = default!;
        public int user_id { get; set; }
        public Dictionary<string, object> detail { get; set; } = default!;
        public PriorityMessage priority { get; set; } = default;
        
        public static Message Create(int entityId, int userId, decimal transferAmount)
        {
            var message = new Message
            {
                entity_id = entityId,
                action = PushType.InWeb,
                event_type = "topup.created",
                user_id = userId,
                detail = new Dictionary<string, object> 
                {
                    {"entity_id", entityId},
                    {"user_id", userId},
                    {"transfer_amount", transferAmount}
                },
                priority = PriorityMessage.High             
            };

            return message;         
        }
    }
    public enum PushType
    {
        InWeb = 0,
        Email = 1,
        Sms = 2
    }
    public enum PriorityMessage
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }

    public class CreateTopup : IConsumer<TopupCreated>
    {
        private readonly UserDbContext _userDbContext;
        private readonly IBusPublisher _publisher;
        private readonly ILogger<CreateTopup> _logger;
        public CreateTopup(UserDbContext userDbContext, IBusPublisher publisher, ILogger<CreateTopup> logger)
        {
            _userDbContext = userDbContext;
            _publisher = publisher; 
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TopupCreated> context)
        {
            _logger.LogCritical($"consumer for {nameof(CreateTopup)} is processing ...");

            if(context.Message is null)
            {
                throw new Exception("Message cannot be null or empty");
            }

            var userEntity = await _userDbContext.Users.SingleOrDefaultAsync(x => x.Username == context.Message.username);

            if(userEntity is null)
            {
                throw new Exception("User cannot be null or empty");
            }

            userEntity.Balance += context.Message.transferAmount;

            //note: dictionary<string,object> / string => message global
            var newMessage = Message.Create(context.Message.transId, userEntity.Id, context.Message.transferAmount);

            //note: processor after
            await _userDbContext.SaveChangesAsync();

            await _publisher.SendAsync(new MessageCreated(Guid.NewGuid(), "topup.created",JsonSerializer.Serialize(newMessage)));
        }
    }
}
