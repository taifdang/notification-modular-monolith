using Hookpay.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Contracts;

public record MessageContracts(Guid correlationId,string eventType,string payload) : IIntegrationEvent;
public record MessageAllContracts(Guid correlationId, int? userId, string title, string body) : IIntegrationEvent;
public record MessagePersonalContracts(Guid correlationId, int? userId, string title, string body) : IIntegrationEvent;
//public record MessagePersonalContracts(List<MessageEvent>? messages) : IIntegrationEvent;
public record MessageEvent(Guid correlationId, int? userId, string title, string body);