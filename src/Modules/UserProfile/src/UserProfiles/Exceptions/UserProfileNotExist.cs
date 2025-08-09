using BuildingBlocks.Exception;
using System.Net;

namespace UserProfile.UserProfiles.Exceptions;
public class UserProfileNotExist : AppException
{
    public UserProfileNotExist() : base("Not find UserProfile, try late again")
    {
    }
}
