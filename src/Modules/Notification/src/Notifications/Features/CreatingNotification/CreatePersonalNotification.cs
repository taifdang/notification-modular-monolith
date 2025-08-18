using BuildingBlocks.Constants;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notification.Data;
using System.Text.Json;
using UserPreference;

namespace Notification.Notifications.Features.CreatingNotification;

public record CreatePersonalNotification(Guid Id) : InternalCommand;
public record PersonalNotificationCreatedDomainEvent(Guid Id) : IDomainEvent;
public class CreatePersonalNotificationHandler : ICommandHandler<CreatePersonalNotification>
{
    private readonly NotificationDbContext _notificationDbContext;  
    private readonly UserPreferenceGrpcService.UserPreferenceGrpcServiceClient _grpcService;
    private readonly IMapper _mapper;

    public CreatePersonalNotificationHandler(NotificationDbContext notificationDbContext,
        UserPreferenceGrpcService.UserPreferenceGrpcServiceClient grpcService, IMapper mapper)
    {
        _notificationDbContext = notificationDbContext;
        _grpcService = grpcService;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreatePersonalNotification request, CancellationToken cancellationToken)
    {
        var recipient =
            await _notificationDbContext.Recipients.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);

        if (recipient is null)
            throw new Exception("Not find recipient");

        //get userPreference
        var userPreference = _grpcService.GetById(
            new GetByIdRequest { Id = recipient.UserId.ToString() },
            cancellationToken: cancellationToken);

        //check userPreference(beta) + message formating = message => queue => channelProcessor       
        //ex: email,sms,inapp => generate 3 message with 3 channel type => queue/ worker=>queue =>channelProcessor
        var preference = JsonSerializer.Deserialize<NotificationConstant.Preferences>(userPreference.UserPreferenceDto.Preference);

        //temp
        var optOut = new Dictionary<string, bool>
        {
            { "email", true },
            { "sms", false },
            { "push", true }
        };

        foreach (var item in optOut)
        {
            if (item.Value)
            {
                _notificationDbContext.NotificationDeliveries.Add(new NotificationDeliveries.Model.NotificationDelivery());
            }
        }

        return Unit.Value;
    }
}
