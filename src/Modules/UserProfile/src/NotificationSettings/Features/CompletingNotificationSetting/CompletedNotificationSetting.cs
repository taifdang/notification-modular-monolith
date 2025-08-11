using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfile.Data;
using UserProfile.NotificationSettings.Dtos;
using UserProfile.NotificationSettings.ValueObject;
using UserProfile.UserProfiles.Exceptions;

namespace UserProfile.NotificationSettings.Features.CompletingNotificationSetting;
public record CompletedNotificationSetting(Guid UserId, string Preference) 
    : ICommand<CompletedNotificationSettingResult>, IInternalCommand;

public record NotificationSettingRegistrationCompletedDomainEvent(Guid Id, Guid UserId, string Preference, bool IsDeleted) 
    : IDomainEvent;

public record CompletedNotificationSettingResult(NotificationSettingDto notificationSettingDto);

public record CompleteNotificationSettingResponseDto(NotificationSettingDto NotificationSettingDto);

public record CompleteNotificationSettingResquestDto(Guid UserId, string Preference);

[ApiController]
public class CompleteNotificationSettingEndpoint(IMapper mapper, IMediator mediator) : ControllerBase 
{
    [HttpPost("register-notification-setting")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<CompletedNotificationSettingResult> RegisterNotificationSetting(CompleteNotificationSettingResquestDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CompletedNotificationSetting>(request);

        var result = await mediator.Send(command, cancellationToken);

        return result;
    }
}

public class CompleteNotificationSettingValidator : AbstractValidator<CompletedNotificationSetting>
{
    public CompleteNotificationSettingValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Please enter UserId");
        RuleFor(x => x.Preference).NotEmpty().WithMessage("Please enter Preference");
        //RuleFor(x => x).Custom((x, context) =>
        //{
        //    if (ValidateJsonObject.IsValidJson(x.Preference))
        //    {
        //        context.AddFailure(nameof(x.Preference), "Preference is invalid json format");
        //    }
        //});
    }
}

public class CompleteNotificationSettingHandler : ICommandHandler<CompletedNotificationSetting, CompletedNotificationSettingResult>
{ 
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IMapper _mapper;

    public CompleteNotificationSettingHandler(UserProfileDbContext userProfileDbContext, IMapper mapper)
    {
        _userProfileDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<CompletedNotificationSettingResult> Handle(CompletedNotificationSetting request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new Exception("");

        var notificationSetting = await _userProfileDbContext.NotificationSettings
            .SingleOrDefaultAsync(x => x.UserId.Value == request.UserId, cancellationToken);

        if (notificationSetting is null)
            throw new UserIdNotExistException(request.UserId);

        notificationSetting.CompletedRegisterNotificationSetting(notificationSetting.Id, notificationSetting.UserId,
            Preference.Of(notificationSetting.Preference));

        var updateNotificationSetting = (_userProfileDbContext.NotificationSettings.Update(notificationSetting)).Entity;

        var notificationSettingDto = _mapper.Map<NotificationSettingDto>(updateNotificationSetting);

        return new CompletedNotificationSettingResult(notificationSettingDto);

    }
}

