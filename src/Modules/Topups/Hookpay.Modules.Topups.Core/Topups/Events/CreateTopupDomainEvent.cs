using Hookpay.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Events;

public record CreateTopupDomainEvent(string username,decimal tranferAmount):IDomainEvent;

