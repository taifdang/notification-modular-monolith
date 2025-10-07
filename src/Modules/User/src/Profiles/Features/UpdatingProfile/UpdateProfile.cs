using Ardalis.GuardClauses;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Data;
using User.Profiles.Dtos;
using User.Profiles.Enums;
using User.Profiles.Exceptions;
using User.Profiles.ValueObjects;

namespace User.Profiles.Features.UpdatingProfile;
public record UpdateProfile(Guid UserId, GenderType GenderType, int Age): ICommand<UpdateProfileResult>, IInternalCommand;
public record ProfileUpdatedDomainEvent(Guid Id, Guid UserId, string UserName, string Name,
        string email, GenderType GenderType, int Age, bool IsDeleted = false) : IDomainEvent;

public record UpdateProfileResult(ProfileDto UserProfileDto);
public record UpdateProfileRequestDto(Guid UserId, GenderType GenderType, int Age);
public record UpdateProfileResponseDto(ProfileDto UserProfileDto);

[ApiController]
[Route("api/user")]
public class UpdateProfileEndpoint(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPut("profile")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<UpdateProfileResponseDto> RegisterUserProfile(UpdateProfileRequestDto request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateProfile>(request);
        var result = await mediator.Send(command, cancellationToken);

        var response = result.Adapt<UpdateProfileResponseDto>();

        return response;
    }
}

public class UpdateProfileValidator : AbstractValidator<UpdateProfile>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Please enter UserId");
        RuleFor(x => x.GenderType).Must(r => r.GetType().IsEnum &&
                                             r == Enums.GenderType.Male ||
                                             r == Enums.GenderType.Female ||
                                             r == Enums.GenderType.Unknown);
        RuleFor(x => x.Age).GreaterThan(0).WithMessage("Age is not a negative value");
    }
}

public class UpdateProfileHandler : ICommandHandler<UpdateProfile, UpdateProfileResult>
{
    private readonly UserDbContext _userDbContext;
    private readonly IMapper _mapper;

    public UpdateProfileHandler(UserDbContext userProfileDbContext, IMapper mapper)
    {
        _userDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<UpdateProfileResult> Handle(UpdateProfile request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var userProfile = await _userDbContext.Profiles
            .SingleOrDefaultAsync(x => x.UserId.Value == request.UserId, cancellationToken);

        if (userProfile is null)
        {
            throw new UserNotExistException();
        }

        userProfile.Update(userProfile.Id, userProfile.UserId, userProfile.UserName, userProfile.Name,
            userProfile.Email, request.GenderType, Age.Of(request.Age));

        var updateUserProfile = _userDbContext.Profiles.Update(userProfile).Entity;

        var userProfileDto = _mapper.Map<ProfileDto>(updateUserProfile);

        return new UpdateProfileResult(userProfileDto);
    }
}
