

using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildingBlocks.Web;

public interface ICurrentUserProvider
{
    Guid? GetCurrentUserId();
}
public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly HttpContextAccessor _httpContextAccessor;
    public CurrentUserProvider(HttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentUserId()
    {
        var nameIdentifier = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        Guid.TryParse(nameIdentifier, out var userId);

        return userId;
    }
}
