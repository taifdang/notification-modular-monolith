
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Configurations;

public class ValidateGrantType : IOpenIddictServerHandler<OpenIddictServerEvents.ValidateTokenRequestContext>
{
    public async ValueTask HandleAsync(OpenIddictServerEvents.ValidateTokenRequestContext context)
    {
        if (!context.Request.IsPasswordGrantType())
        {
            context.Reject(
                error: "UnsupportdGrantType",
                description: "Only password grant type");
            return;
        }

        if (string.IsNullOrWhiteSpace(context.Request.Username) ||
            string.IsNullOrWhiteSpace(context.Request.Password))
        {
            context.Reject(
                error: Errors.InvalidRequest,
                description: "Missing credentials.");
        }     
    }
}
