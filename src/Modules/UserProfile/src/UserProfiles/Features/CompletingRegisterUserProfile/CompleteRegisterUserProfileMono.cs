using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfile.Data;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.UserProfiles.Features.CompletingRegisterUserProfile;

public record CompleteRegisterUserProfileMonoCommand(Guid Id, Guid UserId, string Name,
    Enums.GenderType GenderType, int Age, bool IsDeleted = false) : InternalCommand;

internal class CompleteRegisterUserProfileMonoHandler : ICommandHandler<CompleteRegisterUserProfileMonoCommand>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IMapper _mapper;

    public CompleteRegisterUserProfileMonoHandler(UserProfileDbContext userProfileDbContext, IMapper mapper)
    {
        _userProfileDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CompleteRegisterUserProfileMonoCommand request, CancellationToken cancellationToken)
    {
        var userProfileRequest = _mapper.Map<Model.UserProfile>(request);

        var userProfile = await _userProfileDbContext.UserProfiles.AsQueryable()
           .FirstOrDefaultAsync(x => x.UserId.Value == request.UserId, cancellationToken);

        if (userProfile is not null)
        {
            await _userProfileDbContext.UserProfiles
                .Where(x => x.Id.Value == UserProfileId.Of(request.Id))
                .ExecuteUpdateAsync(x => x
                    .SetProperty(a => a.Id, UserProfileId.Of(request.Id))
                    .SetProperty(a => a.UserId, request.UserId)
                    .SetProperty(a => a.Name, request.Name)
                    .SetProperty(a => a.GenderType, request.GenderType)
                    .SetProperty(a => a.Age, request.Age)
                    .SetProperty(a => a.IsDeleted, request.IsDeleted),
                    cancellationToken: cancellationToken);
        }
        else
        {
            await _userProfileDbContext.UserProfiles.AddAsync(userProfileRequest, cancellationToken);

            await _userProfileDbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
