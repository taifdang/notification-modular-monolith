
using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
using BuildingBlocks.Web;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Setting.Data;
using Setting.NotificationPreferences.Model;
using Setting.NotificationPreferences.ValueObjects;

namespace Setting.NotificationPreferences.Features.UpdatingNotificationPreference;
public record UpdateNotificationPreference(ChannelType Channel, bool IsOptOut) : ICommand<UpdateNotificationPreferenceResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record UpdateNotificationPreferenceResult(Guid Id);

//public record NotificationPreferenceUpdatedDomainEvent(Guid Id, Guid UserId, ChannelType Channel, bool IsOptOut) : IDomainEvent;

public record UpdateNotificationPreferenceRequestDto(ChannelType Channel, bool IsOptOut);
public record UpdateNotificationPreferenceResponseDto(Guid Id);

[ApiController]
[Route("api/settings")]
public class UpdateNotificationPreferenceEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UpdateNotificationPreferenceEndpoint(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPut("notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Result<UpdateNotificationPreferenceResponseDto>> Update(
        UpdateNotificationPreferenceRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<UpdateNotificationPreference>(request), cancellationToken);
        return result is null
            ? Result<UpdateNotificationPreferenceResponseDto>.Failure("Update failed")
            : Result<UpdateNotificationPreferenceResponseDto>.Success(result.Adapt<UpdateNotificationPreferenceResponseDto>());
    }
}
public class UpdateNotificationPreferenceValidator : AbstractValidator<UpdateNotificationPreference>
{
    public UpdateNotificationPreferenceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Channel).Must(p => (p.GetType().IsEnum) && 
                                     p == ChannelType.InApp || 
                                     p == ChannelType.Email || 
                                     p == ChannelType.Push)
            .WithMessage("Channel must be Email, Sms or Push");
    }
}

internal class UpdateNotificationPreferenceCommandHandler : ICommandHandler<UpdateNotificationPreference, UpdateNotificationPreferenceResult>
{
    private readonly SettingDbContext _settingDbContext;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IEventDispatcher _eventDispatcher;

    public UpdateNotificationPreferenceCommandHandler(
        SettingDbContext settingDbContext,
        ICurrentUserProvider currentUserProvider,
        IEventDispatcher eventDispatcher)
    {
        _settingDbContext = settingDbContext;
        _currentUserProvider = currentUserProvider;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<UpdateNotificationPreferenceResult> Handle(UpdateNotificationPreference command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));
        var userId = _currentUserProvider.GetCurrentUserId();
        Guard.Against.Null(userId, nameof(userId));

        var preference = await _settingDbContext.NotificationPreferences
            .FirstOrDefaultAsync(x => x.UserId.Value == userId.Value && x.Channel == command.Channel, cancellationToken);

        if (preference is null)
        {
            preference = NotificationPreference.Create(
                NotificationPreferenceId.Of(command.Id),
                UserId.Of(userId.Value),
                command.Channel,
                command.IsOptOut
            );
            await _settingDbContext.NotificationPreferences.AddAsync(preference, cancellationToken);
        }
        else
        {
            preference.UpdateOptOut(command.IsOptOut);
        }

        await _settingDbContext.SaveChangesAsync(cancellationToken);

        //await _eventDispatcher.SendAsync(
        //    new NotificationPreferenceUpdatedDomainEvent(preference.Id.Value, userId.Value, command.Channel, command.IsOptOut),
        //    typeof(IInternalCommand));

        return new UpdateNotificationPreferenceResult(preference.Id.Value);
    }
}
