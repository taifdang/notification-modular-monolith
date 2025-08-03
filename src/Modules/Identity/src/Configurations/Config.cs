
using Identity.Identity.Constants;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
namespace Identity.Configurations;

public static class Config
{
    public static IEnumerable<string> ApiScopes =>
        new List<string>
        {
            new(Constants.StandardScope.TopupApi),
            new(Constants.StandardScope.NotificationApi),
            new(Constants.StandardScope.ProfileApi),
            new(Constants.StandardScope.IdentityApi),
            new(Constants.StandardScope.NotificationModularMonolith),
            new(OpenIddictConstants.Scopes.OfflineAccess)
        };

    public static IEnumerable<string> IdentityResources =>
        new List<string>
        {
            new(OpenIddictConstants.Permissions.Scopes.Email),
            new(OpenIddictConstants.Permissions.Scopes.Profile)
        };

    public static OpenIddictApplicationDescriptor GetScopeCustom =>
        new OpenIddictApplicationDescriptor
        {
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,

                Permissions.GrantTypes.AuthorizationCode,
                Permissions.ResponseTypes.Code,

                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,

                Permissions.Prefixes.Scope + Constants.StandardScope.TopupApi,
                Permissions.Prefixes.Scope + Constants.StandardScope.ProfileApi,
                Permissions.Prefixes.Scope + Constants.StandardScope.NotificationApi,
                Permissions.Prefixes.Scope + Constants.StandardScope.NotificationModularMonolith
            }
        };
}
