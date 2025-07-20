using Hookpay.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Contracts;

public record TopupContracts(int transId,string username,decimal tranferAmount):IIntegrationEvent;
public record TopupCreated(int transId, string username, decimal transferAmount) : IIntegrationEvent;


