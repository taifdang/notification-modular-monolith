using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Dtos;
using Hookpay.Shared.Core.Pagination;
using MapsterMapper;
using MediatR;

namespace Hookpay.Modules.Users.Core.Users.Features.GetAvailableUsers;

public record GetAvailableUsers (int pageNumber, int pageSize ) : IRequest<GetAvailableUsersResult>;

public record GetAvailableUsersResult(
    List<UserDto> UserDtos,
    int PageNumber,
    int PageSize,
    int TotalPage,
    int TotalItem);

public record GetAvailableUsersReponse(
    List<UserDto> UserDtos, 
    int PageNumber, 
    int PageSize,
    int TotalPage,
    int TotalItem);

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
        if(request.pageNumber <= 0 || request.pageSize <=0)
        {
            throw new Exception("PageNumber || PageSize is invalid");
        } 

        var query =  _userDbContext.Users.AsQueryable()
            .Where(x => 
                !x.IsDeleted && 
                x.Status == Enums.UserStatus.Active && 
                x.UserSetting.AllowNotification == true)
            .OrderBy(x => x.Id);
            
        var user = await PaginationExtensions.GetPagedData(
            query,
            request.pageNumber,
            request.pageSize, 
            cancellationToken);

        if (!user.Results.Any())
        {
            throw new Exception("Empty");
        }

        var userDtos = _mapper.Map<List<UserDto>>(user.Results);   
        
        return new GetAvailableUsersResult(
            userDtos,
            user.PageNumber,
            user.PageSize,
            user.TotalPage,
            user.TotalItem);
    }
}
