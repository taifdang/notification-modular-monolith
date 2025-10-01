using BuildingBlocks.Contracts;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfile.Data;
using UserProfile.UserPreferences.Dtos;
using UserProfile.UserProfiles.Exceptions;

namespace UserProfile.UserPreferences.Features.CompletingUserPreference;
public record CompletedUserPreference(Guid UserId, List<Dtos.PreferenceDto> Preferences) 
    : ICommand<CompletedUserPreferenceResult>, IInternalCommand;

public record UserPreferenceRegistrationCompletedDomainEvent(Guid Id, Guid UserId, ChannelType channel,
        bool isOptOut, bool IsDeleted) 
    : IDomainEvent;

public record CompletedUserPreferenceResult(UserPreferenceDto notificationSettingDto);

public record CompleteUserPreferenceResponseDto(UserPreferenceDto NotificationSettingDto);

public record CompleteUserPreferenceResquestDto(Guid UserId, List<Dtos.PreferenceDto> Preferences);

[ApiController]
[Route("api/preference")]
public class CompleteUserPreferenceEndpoint(IMapper mapper, IMediator mediator) : ControllerBase 
{
    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<CompletedUserPreferenceResult> RegisterNotificationSetting(CompleteUserPreferenceResquestDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CompletedUserPreference>(request);

        var result = await mediator.Send(command, cancellationToken);

        return result;
    }
}

public class CompleteUserPreferenceValidator : AbstractValidator<CompletedUserPreference>
{
    public CompleteUserPreferenceValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Please enter UserId");
        RuleFor(x => x.Preferences).NotEmpty().WithMessage("Please enter Preference");
        //RuleFor(x => x).Custom((x, context) =>
        //{
        //    if (ValidateJsonObject.IsValidJson(x.Preference))
        //    {
        //        context.AddFailure(nameof(x.Preference), "Preference is invalid json format");
        //    }
        //});
    }
}

public class CompleteUserPreferenceHandler : ICommandHandler<CompletedUserPreference, CompletedUserPreferenceResult>
{ 
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IMapper _mapper;

    public CompleteUserPreferenceHandler(UserProfileDbContext userProfileDbContext, IMapper mapper)
    {
        _userProfileDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<CompletedUserPreferenceResult> Handle(CompletedUserPreference request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new Exception("");

        //var notificationSetting = await _userProfileDbContext.UserPreferences
        //    .SingleOrDefaultAsync(x => x.UserId.Value == request.UserId, cancellationToken);

        //if (notificationSetting is null)
        //    throw new UserIdNotExistException(request.UserId);

        //notificationSetting.CompletedRegisterNotificationSetting(notificationSetting.Id, notificationSetting.UserId,
        //    Preference.Of(notificationSetting.Preference));

        //var updateNotificationSetting = (_userProfileDbContext.UserPreferences.UpdateRange(notificationSetting)).Entity;

        //var notificationSettingDto = _mapper.Map<UserPreferenceDto>(updateNotificationSetting);

        var existingPreferences = await _userProfileDbContext.UserPreferences
            .Where(x => x.UserId.Value == request.UserId)
            .ToListAsync(cancellationToken);

        if (!existingPreferences.Any())
            throw new UserIdNotExistException(request.UserId);

        foreach (var dto in request.Preferences)
        {
            var record = existingPreferences.FirstOrDefault(x => x.Channel == dto.Channel);

            if (record != null)
            {
                record.UpdateOptOut(dto.IsOptOut);
            }

        }
        await _userProfileDbContext.SaveChangesAsync(cancellationToken);

        var notificationSettingDto = existingPreferences.Adapt<UserPreferenceDto>();

        return new CompletedUserPreferenceResult(notificationSettingDto);   

    }
}

