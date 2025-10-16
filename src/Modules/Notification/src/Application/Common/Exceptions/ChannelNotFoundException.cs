
using BuildingBlocks.Exception;

namespace Notification.Application.Common.Exceptions;

public class ChannelNotFoundException : DomainException
{
    public ChannelNotFoundException() : base("Channel not found")
    {
    }
}
