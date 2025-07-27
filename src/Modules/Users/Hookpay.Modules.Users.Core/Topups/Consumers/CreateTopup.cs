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
    public enum PushType
    {
        InApp = 0,
        Email = 1,
        Sms = 2
    }
    public enum MessagePriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }
    public class MessagePayload
    {   
        public int EntityId { get; set; } //ex: userId/topupId,...
        public string? Target { get; set; } //ex: deviceToken/userId/Email
        public string? EventType { get; set; }
        public PushType PushType { get; set; }
        public Dictionary<string, object?>? Data { get; set; } = new Dictionary<string, object?>();
        public MessagePriority Priority { get; set; }

        public static MessagePayload Create(
            int userId,
            string target,
            string eventType,
            int topupId,
            decimal transferAmount
            )
        {
            var payload = new MessagePayload
            {
                EntityId = userId,
                Target = userId.ToString(),
                EventType = eventType,
                PushType = PushType.InApp,
                Data =  GetMetaData(topupId, transferAmount),
                Priority = MessagePriority.High
            };

            return payload;
        }

        private static Dictionary<string, object?>? GetMetaData(
            int topupId, 
            decimal transferAmount)
        {
            var metaData = new Dictionary<string, object?>();

            metaData.Add("TopupId", topupId);
            metaData.Add("TransferAmount", transferAmount);

            return metaData;
        }
    }
    public class CreateTopup : IConsumer<TopupCreated>
    {
        private readonly UserDbContext _userDbContext;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILogger<CreateTopup> _logger;
        public CreateTopup(
            UserDbContext userDbContext,
            IEventDispatcher eventDispatcher, 
            ILogger<CreateTopup> logger
         
            )
        {
            _userDbContext = userDbContext;
            _eventDispatcher = eventDispatcher; 
            _logger = logger;      
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

            var newMessage = MessagePayload.Create(
                userEntity.Id,
                string.Empty,
                nameof(TopupCreated), 
                context.Message.transId,
                context.Message.transferAmount);
                                     
            await _userDbContext.SaveChangesAsync();

            await _eventDispatcher.SendAsync(
                new MessageCreated(Guid.NewGuid(), nameof(TopupCreated), JsonSerializer.Serialize(newMessage)));          
        }
    }
}
