using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;
public class InvalidBalanceException : AppException
{
    public InvalidBalanceException() : base("Balance is not a negative value")
    {
    }
}
