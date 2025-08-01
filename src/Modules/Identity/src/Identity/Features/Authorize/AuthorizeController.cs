
using Identity.Identity.Models;
using MassTransit.Internals;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Identity.Features.Authorize;

[ApiController]
public class AuthorizeController : ControllerBase
{
    private static ClaimsIdentity Identity = new ClaimsIdentity();
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AuthorizeController(
        IOpenIddictApplicationManager applicationManager, 
        IOpenIddictAuthorizationManager authorizationManager, 
        IOpenIddictScopeManager scopeManager,
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _applicationManager = applicationManager;
        _authorizationManager = authorizationManager;
        _scopeManager = scopeManager;
        _signInManager = signInManager;
        _userManager = userManager;
    }      

    [HttpPost("connect/token")]
    public async Task<IActionResult> ConnectToken()
    {
        try
        {
            var openIdConnectRequest = HttpContext.GetOpenIddictServerRequest() ?? 
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved");

            Identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                Claims.Name,
                Claims.Role);

            User? user = null;
            AuthenticationProperties properties = new();

            if (openIdConnectRequest.IsClientCredentialsGrantType())
            {
                throw new NotImplementedException();
            }
            else if (openIdConnectRequest.IsPasswordGrantType())
            {
                user = await _userManager.FindByNameAsync(openIdConnectRequest.Username);

                if (user == null)
                {
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "User does not exist"
                    });
                }

                if (!await _signInManager.CanSignInAsync(user) || (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user)))
                {
                    // Return bad request is the user can't sign in
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = OpenIddictConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user cannot sign in."
                    });
                }

                //Validate
                var result = await _signInManager.PasswordSignInAsync(user.UserName, openIdConnectRequest.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    if (result.IsNotAllowed)
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "User not allowed to login. Please confirm your email"
                        });
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "User requires 2F authentication"
                        });
                    }

                    if (result.IsLockedOut)
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "User is locked out"
                        });
                    }
                    else
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "Username or password is incorrect"
                        });
                    }
                }

                if (_userManager.SupportsUserLockout)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                //Scope
                Identity.SetScopes(openIdConnectRequest.GetScopes());

                //Resource
                Identity.SetResources(await _scopeManager.ListResourcesAsync(Identity.GetScopes()).ToListAsync());

                Identity.AddClaim(new Claim(Claims.Subject, user.Id.ToString()));
                Identity.AddClaim(new Claim(Claims.Audience, "Resourse"));

                Identity.SetDestinations(GetDestinations);
            }
            else if (openIdConnectRequest.IsRefreshTokenGrantType())
            {
                throw new NotImplementedException();
            }
            else
            {
                return BadRequest(new
                {
                    error = Errors.UnsupportedGrantType,
                    error_description = "The specified grant type is not supported."
                });
            }
            var signInResult = SignIn(new ClaimsPrincipal(Identity), properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            return signInResult;
        }
        catch (Exception ex)
        {
            return BadRequest(new OpenIddictResponse()
            {
                Error = Errors.ServerError,
                ErrorDescription = "Invalid login attempt"
            });
        }      
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
       
        return claim.Type switch
        {
            Claims.Name or
            Claims.Subject
               => new[] { Destinations.AccessToken, Destinations.IdentityToken },

            _ => new[] { Destinations.AccessToken },
        };
    }
}
