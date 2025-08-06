using Identity.Identity.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Configurations;

public  class UserValidator : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserValidator(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("/connect/token")]
    public async Task<IActionResult> Token()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var user = await _userManager.FindByNameAsync(request.Username!);
        if (user == null || await _userManager.IsLockedOutAsync(user))
            return NotFound("Invalid credentials!");

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password!, false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return BadRequest("Bad request!");

        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        if (!principal.HasClaim(c => c.Type == Claims.Subject))
        {
            var identity = (ClaimsIdentity)principal.Identity!;
            identity.AddClaim(new Claim(Claims.Subject, user.Id.ToString()));
        }

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(Destinations.AccessToken);
        }

        principal.SetAccessTokenLifetime(TimeSpan.FromMinutes(60));
    
        var scope = new[]
        {         
            Scopes.Profile,
            //Scopes.OpenId,           
            //Scopes.OfflineAccess,
        };

        principal.SetScopes(scope);

        var props = new AuthenticationProperties(new Dictionary<string, string?>
        {            
                {"language", "en"},
                {"lastLoginDateTime", DateTime.Now.ToString()},               
        });
        return SignIn(principal, props, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);      
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

    #region version sample
    //var identity = new ClaimsIdentity(
    //        OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
    //        Claims.Name,
    //        Claims.Role);

    //identity.SetScopes(request.GetScopes());

    //identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

    //identity.AddClaim(new Claim(Claims.Subject, user.Id.ToString()));
    //identity.AddClaim(new Claim(Claims.Name, user.UserName));

    //identity.SetDestinations(_ => new[] { Destinations.AccessToken });

    //return SignIn(new ClaimsPrincipal(identity), props, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    #endregion
}
