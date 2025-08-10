using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfile.Data;
using UserProfile.NotificationSettings.ValueObject;

namespace UserProfile.NotificationSettings.Features.CompletingNotificationSetting;

public record CompletedNotificationSettingMonoCommand(Guid Id, Guid UserId, string Preference, bool IsDeleted = false)
    : InternalCommand;

public record CompletedNotificationSettingMonoHandler : ICommandHandler<CompletedNotificationSettingMonoCommand>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IMapper _mapper;

    public CompletedNotificationSettingMonoHandler(UserProfileDbContext userProfileDbContext, IMapper mapper)
    {
        _userProfileDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CompletedNotificationSettingMonoCommand request, CancellationToken cancellationToken)
    {
        var notificationSettingRequest = _mapper.Map<Model.NotificationSetting>(request);

        var notificationSetting = await _userProfileDbContext.NotificationSettings.AsQueryable()
           .FirstOrDefaultAsync(x => x.UserId.Value == request.UserId, cancellationToken);

        if (notificationSetting is not null)
        {
            await _userProfileDbContext.NotificationSettings
                .Where(x => x.Id.Value == NotificationSettingId.Of(request.Id))
                .ExecuteUpdateAsync(x => x
                    .SetProperty(a => a.Id, NotificationSettingId.Of(request.Id))
                    .SetProperty(a => a.UserId, request.UserId)
                    .SetProperty(a => a.Preference, request.Preference)
                    .SetProperty(a => a.IsDeleted, request.IsDeleted),
                    cancellationToken: cancellationToken);
        }
        else
        {
            await _userProfileDbContext.NotificationSettings.AddAsync(notificationSettingRequest, cancellationToken);

            await _userProfileDbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}

