using BuildingBlocks.Exception;

namespace User.Preferences.Exceptions;

public class InvalidPreferenceIdException : DomainException
{
    public InvalidPreferenceIdException(Guid Id) : base($"PreferenceId: {Id} is invalid")
    {
    }
}
