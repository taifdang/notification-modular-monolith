﻿
using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;

namespace Topup.Topups;

public sealed class IdentityEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            _ => null
        };
    }
}
