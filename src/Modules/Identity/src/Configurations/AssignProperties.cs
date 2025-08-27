
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Server;

namespace Identity.Configurations;

public class AssignProperties : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignInContext>
{
    public ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignInContext context)
    {
        var properties = context.Transaction?.GetProperty<AuthenticationProperties>(
                           typeof(AuthenticationProperties).FullName);
      
        context.Response["language"] = properties?.GetString("language");
        context.Response["last_update_at"] = properties?.GetString("lastLoginDateTime");

        return default;
    }
}
