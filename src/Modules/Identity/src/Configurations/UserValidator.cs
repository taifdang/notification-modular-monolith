using Identity.Identity.Constants;
using Identity.Identity.Models;
using MassTransit.Internals;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Configurations;

public  class UserValidator : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    public UserValidator(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IOpenIddictScopeManager scopeManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _scopeManager = scopeManager;
    }

    [HttpPost("/connect/token")]
    public async Task<IActionResult> Token()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        try
        {
            var user = await _userManager.FindByNameAsync(request.Username!);
            if (user == null || await _userManager.IsLockedOutAsync(user))
                return NotFound("Invalid credentials!");

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password!, 
                false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return BadRequest("Bad request!");

            var userId = user!.Id.ToString();

            //create claim-based
            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, 
                Claims.Name, Claims.Role);

            //add the claims
            identity.SetClaim(Claims.Subject, userId)
                    .SetClaim(ClaimTypes.NameIdentifier, userId)
                    .SetClaim(Claims.Name, user.UserName)
                    .SetClaim(Claims.Audience, Constants.StandardScope.NotificationModularMonolith)
                    .SetClaim(Claims.Scope, Constants.StandardScope.NotificationModularMonolith);

            //set resource
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

            identity.SetDestinations(GetDestinations);

            //extra properties
            var props = new AuthenticationProperties(new Dictionary<string, string?>
            {
                    {"language", "en"},
                    {"lastLoginDateTime", DateTime.Now.ToString()},
            });

            return SignIn(new ClaimsPrincipal(identity), props,
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        catch
        {
            throw new NotImplementedException("The specified grant type is not implemented.");
        }
    }
    public static IEnumerable<string> GetDestinations(Claim claim)
    {
        return claim.Type switch
        {
            Claims.Name or
            Claims.Subject 
                when claim.Subject!.HasScope (Scopes.Profile)
                   => new[] {
                       Destinations.AccessToken,
                       Destinations.IdentityToken
                   },

            _ => new[] { Destinations.AccessToken },
        };
    }
}
