using BuildingBlocks.Core.CQRS;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UserProfile.Data;
using UserProfile.UserPreferences.Dtos;
using UserProfile.UserPreferences.Exceptions;

namespace UserProfile.UserPreferences.Features.GettingUserPreferenceById;

public record GetUserPreferenceById(Guid UserId) : IQuery<GetUserPreferenceByIdResult>;

public record GetUserPreferenceByIdResult(UserPreferenceDto UserPreferenceDto);

public record GetUserPreferenceByIdResponse(UserPreferenceDto UserPreferenceDto);

public class GetUserPreferenceByIdHanler : IQueryHandler<GetUserPreferenceById, GetUserPreferenceByIdResult>
{
    private readonly UserProfileDbContext _userProfileDbContext;
    private readonly IMapper _mapper;

    public GetUserPreferenceByIdHanler(UserProfileDbContext userProfileDbContext, IMapper mapper)
    {
        _userProfileDbContext = userProfileDbContext;
        _mapper = mapper;
    }

    public async Task<GetUserPreferenceByIdResult> Handle(GetUserPreferenceById request, CancellationToken cancellationToken)
    {
        var userPreference = await _userProfileDbContext.UserPreferences.AsQueryable()
            .SingleOrDefaultAsync(x => x.UserId == request.UserId &&
                !x.IsDeleted, cancellationToken);

        if(userPreference is null)
        {
            throw new UserPreferenceNotFound();
        }

        var userPreferenceDto = _mapper.Map<UserPreferenceDto>(userPreference);

        return new GetUserPreferenceByIdResult(userPreferenceDto);
    }
}

