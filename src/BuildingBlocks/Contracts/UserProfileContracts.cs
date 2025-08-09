
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record UserProfileRegistrationCompleted(Guid Id) : IIntegrationEvent;
public record UserProfileCreated(Guid Id) : IIntegrationEvent;

