using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;

public class InvalidNameExeption : AppException
{
    public InvalidNameExeption() : base("Name is not empty")
    {
    }
}
