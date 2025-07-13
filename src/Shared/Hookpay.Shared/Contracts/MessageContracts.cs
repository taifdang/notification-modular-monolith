using Hookpay.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Contracts;

public record MessageContracts(Guid correlationId,string eventType,string payload):IIntegrationEvent;
public record MessageEventContracts(Guid correlationId, int userId, string title, string body) : IIntegrationEvent;