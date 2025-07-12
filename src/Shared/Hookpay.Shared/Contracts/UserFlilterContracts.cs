using Hookpay.Shared.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Contracts;

public record UserFlilterContracts(List<int> Ids):IRequest<List<int>>;
