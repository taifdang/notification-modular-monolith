using BuildingBlocks.Exception;

namespace User.Preferences.Exceptions;
public class PreferenceNotFoundException : DomainException
{
    public PreferenceNotFoundException() : base("Not found preference")
    {
    }
}
