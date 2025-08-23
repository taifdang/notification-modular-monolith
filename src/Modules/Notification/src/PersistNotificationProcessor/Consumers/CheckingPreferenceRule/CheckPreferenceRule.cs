
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
        var notification = 
            await _notificationDbContext.Notifications.FirstOrDefaultAsync(x => x.Id == context.Message.Id);

        if (notification is null)
            return;

        //get user reference
        var userReference = _grpcClient.GetById(new GetByIdRequest { Id = context.Message.UserId.ToString()});

        if (userReference is null)
            return;

        var preference = JsonSerializer.Deserialize<NotificationConstant.Preferences>(userReference.UserPreferenceDto.Preference);
        //check rule
        var seletedChannel = NotificationRule.SelectChannels(notification, preference!);

        //***
        await _publishEndpoint.Publish(new NotificationReadyToRender(context.Message.Id, seletedChannel));
        
        
    }
}
