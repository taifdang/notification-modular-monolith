using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Dtos;
using Hookpay.Modules.Users.Core.Users.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Core.Pagination;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Users.Core.Users.Features.GetAvailableUsers;

public record GetAvailableUsers : IRequest<GetAvailableUsersResult>;

public record GetAvailableUsersResult(List<UserDto> UserDtos);

public record GetAvailableUsersHandler : IRequestHandler<GetAvailableUsers, GetAvailableUsersResult>
{
    private readonly UserDbContext _userDbContext;
    private readonly IMapper _mapper;
    public GetAvailableUsersHandler(UserDbContext userDbContext, IMapper mapper)
    {
        _userDbContext = userDbContext;
        _mapper = mapper;
    }
    public async Task<GetAvailableUsersResult> Handle(GetAvailableUsers request, CancellationToken cancellationToken)
    {
        var query =  _userDbContext.Users.AsQueryable()
            .Where(x => 
                !x.IsDeleted && 
                x.Status == Enums.UserStatus.Active && 
                x.UserSetting.AllowNotification == true)
            .OrderBy(x => x.Id);
            
        var user = await PaginationExtensions.GetPagedData(query, 1, 10, cancellationToken);

        if (!user.Results.Any())
        {
            throw new Exception("Empty");
        }

        var userDtos = _mapper.Map<List<UserDto>>(user.Results);   
        
        return new GetAvailableUsersResult(userDtos);
    }
}
