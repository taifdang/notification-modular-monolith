using BuildingBlocks.Constants;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
using HandlebarsDotNet;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notification.Data;
using Notification.NotificationDeliveries.Model;
using System.Text.Json;
using UserPreference;

namespace Notification.Notifications.Features.CreatingNotification;

//ref: https://www.techiehook.com/articles/convert-strings-to-dictionaries-in-csharp
//ref: https://www.tutorialsteacher.com/articles/convert-string-to-enum-in-csharp
public record CreatePersonalNotification(Guid Id,Guid UserId) : InternalCommand;
public record PersonalNotificationCreatedDomainEvent(Guid Id,Guid UserId) : IDomainEvent;
public class CreatePersonalNotificationHandler : ICommandHandler<CreatePersonalNotification>
{
    private readonly NotificationDbContext _notificationDbContext;  
    private readonly UserPreferenceGrpcService.UserPreferenceGrpcServiceClient _grpcService;
    private readonly IMapper _mapper;
    private readonly ITemplateMessage _template;

    public CreatePersonalNotificationHandler(NotificationDbContext notificationDbContext,
        UserPreferenceGrpcService.UserPreferenceGrpcServiceClient grpcService, IMapper mapper, ITemplateMessage template)
    {
        _notificationDbContext = notificationDbContext;
        _grpcService = grpcService;
        _mapper = mapper;
        _template = template;
    }

    public async Task<Unit> Handle(CreatePersonalNotification request, CancellationToken cancellationToken = default)
    {
        var notification =
            await _notificationDbContext.Notifications.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,cancellationToken);

        if (notification is null)
            throw new Exception("Not find notification");

        //get userPreference
        var userPreference = _grpcService.GetById(
            new GetByIdRequest { Id = request.UserId.ToString() },
            cancellationToken: cancellationToken);

        //check userPreference before render message via template      
        var preference = JsonSerializer.Deserialize<NotificationConstant.Preferences>(userPreference.UserPreferenceDto.Preference);

        foreach (var item in preference.optOut.GetType().GetProperties())
        {
            var name = item.Name;
            var value = (bool)item.GetValue(preference.optOut);

            if (value)
            {
                var toEnum = Enum.TryParse<ChannelType>(item.Name, true, out var channelType);
                if (!toEnum)
                    throw new Exception("Not convert string to enum");

                //render message         
                var message = RenderMessage(notification);

                //check item exist
                var notificationDelivery =
                    await _notificationDbContext.NotificationDeliveries.AsQueryable().SingleOrDefaultAsync(
                        x => x.NotificationId == request.Id &&
                             x.NotificationType == notification.NotificationType &&
                             x.ChannelType == channelType &&
                             !x.IsDeleted,
                        cancellationToken);

                if (notificationDelivery is not null)
                    continue;

                var notificaitonEntity = NotificationDelivery.Create(NewId.NextGuid(),request.Id,request.UserId,
                    notification.NotificationType,channelType, message, notification.Priority,0);                                             
                
                await _notificationDbContext.NotificationDeliveries.AddAsync(notificaitonEntity);
            }
        }
        //temp*
        notification.Status = Enums.NotificationStatus.Processed;
        await _notificationDbContext.SaveChangesAsync();

        return Unit.Value;
    }

    public string RenderMessage(Model.Notification notification)
    {
        var source = _template.RenderTemplate(notification.NotificationType.ToString());
        if (string.IsNullOrEmpty(source))
        {
            throw new Exception("Not support template");
            //use template default
        }

        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(notification.MessageContent);
        var template = Handlebars.Compile(source);

        return template(data);

    }
}
