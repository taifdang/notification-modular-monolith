using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserProfile.Data;
using UserProfile.UserProfiles.Exceptions;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.Topups.Consumers.CreatingTopup;
public class CreateTopup : IConsumer<TopupCreated>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger<CreateTopup> _logger;

    public CreateTopup(UserProfileDbContext userProfileDbContext, IEventDispatcher eventDispatcher, ILogger<CreateTopup> logger)
    {
        _userProfileDbContext = userProfileDbContext;
        _eventDispatcher = eventDispatcher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TopupCreated> context)
    {
        _logger.LogInformation($"Consumer of {nameof(TopupCreated)} is started");

        if (context.Message is null)
            return;
        var listUser = await _userProfileDbContext.UserProfiles.ToListAsync();

        var user = await _userProfileDbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserName.Value == context.Message.username);
        if(user is null)
        {
            throw new UserProfileNotExist();
        }
        var currentBalance = user.Balance?.Value ?? 0m;

        // Tính toán số dư mới
        var newBalance = Balance.Of(currentBalance + context.Message.transferAmount);

        // Gán lại cho entity
        user.Balance = newBalance;

        await _userProfileDbContext.SaveChangesAsync();

        var @event = new NotificationEvent()
        {
            requestId = NewId.NextGuid(),
            notificationType = NotificationType.Topup,
            recipient = new RecipientEvent
            {
                userId = user.Id,
            },
            payload = new Dictionary<string, object?>
            {
                {"topupId",context.Message.id},
                {"transferAmount",context.Message.transferAmount}
            },
            metadata = new MetadataEvent
            {
                priority = NotificationPriority.High,
                retries = 3
            }          
        };

        await _eventDispatcher.SendAsync(new NotificationCreated(@event));
    }
}
