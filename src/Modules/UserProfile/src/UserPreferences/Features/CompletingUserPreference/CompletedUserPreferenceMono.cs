using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfile.Data;
using UserProfile.UserPreferences.Model;
using UserProfile.UserPreferences.ValueObject;

namespace UserProfile.UserPreferences.Features.CompletingUserPreference;

public record CompletedUserPreferenceMonoCommand(Guid Id, Guid UserId, string Preference, bool IsDeleted = false)
    : InternalCommand;

public record CompletedUserPreferenceMonoHandler : ICommandHandler<CompletedUserPreferenceMonoCommand>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IMapper _mapper;

    public CompletedUserPreferenceMonoHandler(UserProfileDbContext userProfileDbContext, IMapper mapper)
    {
        _userProfileDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CompletedUserPreferenceMonoCommand request, CancellationToken cancellationToken)
    {
        var userPreferenceRequest = _mapper.Map<Model.UserPreference>(request);

        var userPreference = await _userProfileDbContext.UserPreferences.AsQueryable()
           .FirstOrDefaultAsync(x => x.UserId.Value == request.UserId, cancellationToken);

        if (userPreference is not null)
        {
            await _userProfileDbContext.UserPreferences
                .Where(x => x.Id.Value == UserPreferenceId.Of(request.Id))
                .ExecuteUpdateAsync(x => x
                    .SetProperty(a => a.Id, UserPreferenceId.Of(request.Id))
                    .SetProperty(a => a.UserId, request.UserId)
                    .SetProperty(a => a.Preference, request.Preference)
                    .SetProperty(a => a.IsDeleted, request.IsDeleted),
                    cancellationToken: cancellationToken);
        }
        else
        {
            await _userProfileDbContext.UserPreferences.AddAsync(userPreferenceRequest, cancellationToken);

            await _userProfileDbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}

