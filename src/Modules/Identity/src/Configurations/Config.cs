
using Identity.Identity.Constants;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
namespace Identity.Configurations;

public static class Config
{
    public static string[] ApiScopes =>
        new string[]
        {
            new(Scopes.OpenId),
            new(Scopes.Email),
            new(Scopes.Profile),
            new(Constants.StandardScope.TopupApi),
            new(Constants.StandardScope.NotificationApi),
            new(Constants.StandardScope.ProfileApi),
            new(Constants.StandardScope.IdentityApi),
            new(Constants.StandardScope.NotificationModularMonolith),
            new(Scopes.Roles),
        };

    public static OpenIddictApplicationDescriptor GetClientSeeder =>
        new OpenIddictApplicationDescriptor
        {
            ClientId = "client",
            ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
            ClientType = ClientTypes.Confidential,
            RedirectUris =
            {
               new Uri("https://localhost:7265/connect/token")
            },
            Permissions =
            {
                //endpoint processor
                Permissions.Endpoints.Token,
                Permissions.Endpoints.Authorization,

                //grant_type
                Permissions.GrantTypes.ClientCredentials,
                Permissions.GrantTypes.Password,

                //response_type
                Permissions.ResponseTypes.Code,

                //command: OIDC - openId
                Permissions.ResponseTypes.IdToken,

                //scope
                Permissions.Scopes.Roles,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,

                //custom scope
                //user get id_token
                Permissions.Prefixes.Scope + "openid",
                Permissions.Prefixes.Scope + Constants.StandardScope.TopupApi,
                Permissions.Prefixes.Scope + Constants.StandardScope.ProfileApi,
                Permissions.Prefixes.Scope + Constants.StandardScope.NotificationApi,
                Permissions.Prefixes.Scope + Constants.StandardScope.NotificationModularMonolith,

            },
        };
}
