using BuildingBlocks.Exception;

namespace User.Profiles.Exceptions;

public class InvalidNameExeption : AppException
{
    public InvalidNameExeption() : base("Name is not empty")
    {
    }
}
