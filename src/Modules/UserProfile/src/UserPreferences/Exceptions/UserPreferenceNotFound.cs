using BuildingBlocks.Exception;
using System.Net;

namespace UserProfile.UserPreferences.Exceptions;

public class UserPreferenceNotFound : AppException
{
    public UserPreferenceNotFound() : base("UserPreference not found!", HttpStatusCode.NotFound)
    {
    }
}
