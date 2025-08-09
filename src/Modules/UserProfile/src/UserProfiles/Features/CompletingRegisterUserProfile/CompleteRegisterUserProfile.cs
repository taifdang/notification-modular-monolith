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
using UserProfile.UserProfiles.Dtos;
using UserProfile.UserProfiles.Enums;
using UserProfile.UserProfiles.Exceptions;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.UserProfiles.Features.CompletingRegisterUserProfile;

public record CompleteRegisterUserProfile(Guid UserId, GenderType GenderType, int Age)
    : ICommand<CompleteRegisterUserProfileResult>, IInternalCommand;
public record UserProfileRegistrationCompletedDomainEvent(Guid Id, Guid UserId, string Name,
        GenderType GenderType, int Age, bool IsDeleted = false) : IDomainEvent;

public record CompleteRegisterUserProfileResult(UserProfileDto UserProfileDto);
public record CompleteRegisterUserProfileRequestDto(Guid UserId, GenderType GenderType, int Age);
public record CompleteRegisterUserProfileResponseDto(UserProfileDto UserProfileDto);

[ApiController]
public class CompleteRegisterUserProfileEndpoint(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("register-userprofile")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<CompleteRegisterUserProfileResponseDto> RegisterUserProfile(CompleteRegisterUserProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CompleteRegisterUserProfile>(request);
        var result = await mediator.Send(command, cancellationToken);

        var response = result.Adapt<CompleteRegisterUserProfileResponseDto>();

        return response;
    }   
}

public class CompleteRegisterUserProfileValidator : AbstractValidator<CompleteRegisterUserProfile>
{
    public CompleteRegisterUserProfileValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Please enter UserId");
        RuleFor(x => x.GenderType).Must(r => r.GetType().IsEnum &&
                                             r == Enums.GenderType.Male ||
                                             r == Enums.GenderType.Female ||
                                             r == Enums.GenderType.Unknown);
        RuleFor(x => x.Age).GreaterThan(0).WithMessage("Age is not a negative value");
    }
}

public class CompleteRegisterUserProfileHanlder : ICommandHandler<CompleteRegisterUserProfile, CompleteRegisterUserProfileResult>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IMapper _mapper;

    public CompleteRegisterUserProfileHanlder(UserProfileDbContext userProfileDbContext, IMapper mapper)
    {
        _userProfileDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<CompleteRegisterUserProfileResult> Handle(CompleteRegisterUserProfile request, 
        CancellationToken cancellationToken)
    {

        var userProfile = await _userProfileDbContext.UserProfiles
            .SingleOrDefaultAsync(x => x.UserId.Value == request.UserId, cancellationToken);

        if(userProfile is null)
        {
            throw new UserProfileNotExist();
        }

        userProfile.CompleteRegistrationUserProfile(userProfile.Id, userProfile.UserId, userProfile.Name,
            request.GenderType, Age.Of(request.Age));

        var updateUserProfile = _userProfileDbContext.UserProfiles.Update(userProfile).Entity;

        var userProfileDto = _mapper.Map<UserProfileDto>(updateUserProfile);    

        return new CompleteRegisterUserProfileResult(userProfileDto);

    }
}
