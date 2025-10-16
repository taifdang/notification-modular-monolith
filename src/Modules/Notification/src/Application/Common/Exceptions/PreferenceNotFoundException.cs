
using BuildingBlocks.Exception;

namespace Notification.Application.Common.Exceptions;

public class PreferenceNotFoundException : DomainException
{
    public PreferenceNotFoundException() : base("Preference not found")
    {
    }
}