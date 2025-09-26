
using AutoBogus;
namespace Integration.Test.Fakes;
using global::Identity.Identity.Features.RegisteringNewUser; 

public class FakeRegisterNewUserCommand : AutoFaker<RegisterNewUser>
{
    public FakeRegisterNewUserCommand()
    {
        RuleFor(r => r.UserName, x => "UserTest");
        RuleFor(r => r.Password, _ => "Password@123");
        RuleFor(r => r.ConfirmPassword, _ => "Password@123");
        RuleFor(r => r.Email, _ => "user@test.com");
    }
}
