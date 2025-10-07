namespace User.Preferences.Features.UpdatingPreference;

using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Web;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Data;
using User.Preferences.ValueObjects;

public record UpdatePreference(Guid UserId, ChannelType Channel, bool IsOptOut) : ICommand<UpdatePreferenceResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}
public record UpdatePreferenceResult(Guid Id);

public record UpdatePreferenceDto(ChannelType Channel, bool IsOptOut);
public record UpdatePreferenceResponseDto(Guid Id);

[ApiController]
[Route("api/user")]
public class UpdatePreferenceEndpoint(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPut("preference")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<UpdatePreferenceResponseDto> Update(
        UpdatePreferenceDto request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(mapper.Map<UpdatePreference>(request), cancellationToken);
        var response = result.Adapt<UpdatePreferenceResponseDto>();
        return response;
    }
}
public class UpdateNotificationPreferenceValidator : AbstractValidator<UpdatePreference>
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

internal class UpdatePreferenceCommandHandler : ICommandHandler<UpdatePreference, UpdatePreferenceResult>
{
    private readonly UserDbContext _userDbContext;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IEventDispatcher _eventDispatcher;

    public UpdatePreferenceCommandHandler(
        UserDbContext settingDbContext,
        ICurrentUserProvider currentUserProvider,
        IEventDispatcher eventDispatcher)
    {
        _userDbContext = settingDbContext;
        _currentUserProvider = currentUserProvider;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<UpdatePreferenceResult> Handle(UpdatePreference request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var preference = await _userDbContext.Preferences
            .FirstOrDefaultAsync(x => x.UserId.Value == request.UserId && x.Channel == request.Channel, cancellationToken);

        if (preference is null)
        {
            preference = Model.Preference.Create(PreferenceId.Of(request.Id),UserId.Of(request.UserId), request.Channel, request.IsOptOut);

            await _userDbContext.Preferences.AddAsync(preference, cancellationToken);
        }
        else
        {
            preference.UpdateOptOut(request.IsOptOut);
        }

        await _userDbContext.SaveChangesAsync(cancellationToken);

        return new UpdatePreferenceResult(preference.Id.Value);
    }
}
