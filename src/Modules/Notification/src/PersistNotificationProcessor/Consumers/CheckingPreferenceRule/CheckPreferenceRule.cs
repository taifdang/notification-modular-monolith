
namespace Notification.PersistNotificationProcessor.Consumers.CheckingPreferenceRule;

using BuildingBlocks.Constants;
using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification.Configurations.Rules;
using Notification.Data;
using System.Text.Json;
using System.Threading.Tasks;
using UserPreference;

//note: this consumer check user preference and select channel base on rule
public class CheckPreferenceRule : IConsumer<NotificationCreated>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly UserPreferenceGrpcService.UserPreferenceGrpcServiceClient _grpcClient; 

    public CheckPreferenceRule(NotificationDbContext notificationDbContext,
        UserPreferenceGrpcService.UserPreferenceGrpcServiceClient grpcClient,
        IPublishEndpoint publishEndpoint)
    {
        _grpcClient = grpcClient;
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationCreated> context)
    {
        var @event = context.Message;

        //check notification exist
        var notification = 
            await _notificationDbContext.Notifications.FirstOrDefaultAsync(x => x.Id == @event.Id);

        if (notification is null)
            return;

        var userId = @event.UserId.ToString();

        //call grpc to get user preference
        var userReference = _grpcClient.GetById(new GetByIdRequest { Id = userId });

        if (userReference is null)
            return;

        //var preference = JsonSerializer.Deserialize<NotificationConstant.Preferences>(userReference.UserPreferenceDto.Preference);

        var preference = userReference.UserPreferenceDto.Preference
            .Select(p => 
                new PreferenceDto(
                    (BuildingBlocks.Contracts.ChannelType)p.Channel,
                    p.IsOptOut))
            .ToList();

        //select channel base on rule and user preference
        var seletedChannels = NotificationRule.GetChannels(notification, preference);

        if (!seletedChannels.Any())
            throw new Exception("Occur error when get channels from user preference");

        //save recipient with channel
        foreach (var channel in seletedChannels)
        {
            var recipient = Recipents.Model.Recipient.Create(NewId.NextGuid(), notification.Id,
                 channel, @event.UserId, @event.Email);

            await _notificationDbContext.Recipients.AddAsync(recipient);
        }
        await _notificationDbContext.SaveChangesAsync();
   
        await _publishEndpoint.Publish(new NotificationReadyToRender(@event.Id, Guid.Parse(userReference.UserPreferenceDto.UserId),
            notification.RequestId, notification.NotificationType, notification.Priority,notification.DataSchema, seletedChannels));   
    }
}
