
using Identity.Identity.Models;
using MassTransit.Internals;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace Identity.Configurations;

public class UserValidator : IOpenIddictServerHandler<HandleAuthorizationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<HandleAuthorizationRequestContext>()
            .UseScopedHandler<UserValidator>()    
            .SetOrder(500)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    public ClaimsIdentity Identity = new ClaimsIdentity();
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    public UserValidator(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictAuthorizationManager authorizationManager,
        IOpenIddictScopeManager scopeManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _applicationManager = applicationManager;
        _authorizationManager = authorizationManager;
        _scopeManager = scopeManager;
    }

    public async ValueTask HandleAsync(HandleAuthorizationRequestContext context)
    {
        Console.WriteLine("success>>>>>>>>>>>>>>>>>>>>>>>");
        Identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                ClaimTypes.NameIdentifier,
                ClaimTypes.Name);

        var user = await _userManager.FindByNameAsync(context.Request.Username);

        var signIn = await _signInManager.PasswordSignInAsync(
                user,
                context.Request.Password,
                isPersistent: true,
                lockoutOnFailure: true);

        if (signIn.Succeeded)
        {
            var userId = user!.Id.ToString();
            //generate token    
            Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            Identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            Identity.SetDestinations(GetDestinations);

            var principal = new ClaimsPrincipal(Identity);

            principal.SetScopes(context.Request.GetScopes());

            principal.SetResources(await _scopeManager.ListResourcesAsync(Identity.GetScopes()).ToListAsync());

            context.SignIn(principal);

            

            return;
        }

        context.Reject(
               error: Errors.UnauthorizedClient,
               description: "Invalid username or password.");
        Console.WriteLine("errorr>>>>>>>>>>>>>>>>>>>>>>>");

        return;

    }

    public async Task ValidateAsync()
    {
        var user = await _userManager.FindByNameAsync("");

        var signIn = await _signInManager.PasswordSignInAsync(
                user,
                "",
                isPersistent: true,
                lockoutOnFailure: true);

        if (signIn.Succeeded)
        {
            var userId = user!.Id.ToString();
            //generate token

            
            return;
        }

        //failure
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {

        return claim.Type switch
        {
            ClaimTypes.NameIdentifier or
            ClaimTypes.Name
               => new[] { Destinations.AccessToken, Destinations.IdentityToken },

            _ => new[] { Destinations.AccessToken },
        };
    }
}
