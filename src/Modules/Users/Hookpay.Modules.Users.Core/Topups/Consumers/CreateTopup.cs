using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Topups.Exceptons;
using Hookpay.Modules.Users.Core.Users.Exceptions;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Core;
using Hookpay.Shared.EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hookpay.Modules.Users.Core.Topups.Consumers
{
    public class Message() {
        public int EntityId { get; set; }
        public PushType PushType { get; set; } = default;
        public string EventType { get; set; } = default!;
        public int UserId { get; set; }
        public Dictionary<string, object> MetaData { get; set; } = default!;
        public PriorityMessage Priority { get; set; } = default;
        
        public static Message Create(int entityId, int userId, decimal transferAmount)
        {
            var message = new Message
            {
                EntityId = entityId,
                PushType = PushType.InWeb,
                EventType = nameof(TopupCreated),
                UserId = userId,
                MetaData = new Dictionary<string, object> 
                {
                    {"EntityId", entityId},                
                    {"TransferAmount", transferAmount}
                },
                Priority = PriorityMessage.High             
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
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILogger<CreateTopup> _logger;
        private readonly IBusPublisher _busPublisher;
        public CreateTopup(
            UserDbContext userDbContext,
            IEventDispatcher eventDispatcher, 
            ILogger<CreateTopup> logger,
            IBusPublisher busPublisher
            )
        {
            _userDbContext = userDbContext;
            _eventDispatcher = eventDispatcher; 
            _logger = logger;
            _busPublisher = busPublisher;   
        }

        public async Task Consume(ConsumeContext<TopupCreated> context)
        {
            _logger.LogCritical($"consumer for {nameof(CreateTopup)} is processing ...");

            if (context.Message is null)
            {
                throw new MessageNotFoundException();
            }

            var userEntity = await _userDbContext.Users.SingleOrDefaultAsync(x => x.Username == context.Message.username);

            if (userEntity is null)
            {
                throw new UserAlreadyExistException();
            }
          
            userEntity.Deposit(userEntity.Balance);

            //note: dictionary<string,object> / string => message global
            var newMessage = Message.Create(context.Message.transId, userEntity.Id, context.Message.transferAmount);

            //note: processor after
            await _userDbContext.SaveChangesAsync();

            await _eventDispatcher.SendAsync(
                new MessageCreated(Guid.NewGuid(), nameof(TopupCreated), JsonSerializer.Serialize(newMessage)));

            //await _busPublisher.SendAsync(new MessageCreated(Guid.NewGuid(), nameof(TopupCreated),JsonSerializer.Serialize(newMessage)));
        }
    }
}
