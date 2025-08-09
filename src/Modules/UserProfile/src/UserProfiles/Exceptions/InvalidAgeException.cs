using BuildingBlocks.Exception;
using System.Net;

namespace UserProfile.UserProfiles.Exceptions;

public class InvalidAgeException : AppException
{
    public InvalidAgeException() : base("Name is not empty")
    {
    }
}
