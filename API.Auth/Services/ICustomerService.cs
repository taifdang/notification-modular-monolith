namespace API.Auth.Services
{
    public interface ICustomerService
    {
        string GenerateJwtToken(string username);
    }
}
