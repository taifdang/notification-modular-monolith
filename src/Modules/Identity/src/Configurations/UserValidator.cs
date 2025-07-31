
using Identity.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Configurations;

public class UserValidator
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserValidator(
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
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

}
