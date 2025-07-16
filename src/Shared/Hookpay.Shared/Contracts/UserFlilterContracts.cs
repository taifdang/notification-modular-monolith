using Hookpay.Shared.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Contracts;

public record UserFlilterContracts(List<int> Ids):IRequest<List<int>>;
public record UserOffSetContracts(int pageIndex, int pageSize) : IRequest<List<int>>;
public record UserTotalItem : IRequest<int>;
public record GetUserByPageIndex(int pageIndex,int pageSize) : IRequest<List<int>>;
