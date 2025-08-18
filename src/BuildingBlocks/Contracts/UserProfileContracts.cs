
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record UserProfileRegistrationCompleted(Guid Id) : IIntegrationEvent;
public record UserProfileCreated(Guid Id) : IIntegrationEvent;
public record UserPreferenceResult(Guid Id, Guid UserId, string Preference);

