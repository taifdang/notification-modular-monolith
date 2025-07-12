using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Modules.Topups.Core.Topups.Dao;
using Hookpay.Modules.Topups.Core.Topups.Events;
using Hookpay.Modules.Topups.Core.Topups.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Models;
using Hookpay.Shared.EventBus;
using MassTransit;
using MassTransit.SqlTransport.Topology;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Features;

public class CreateTopupCommandHandler : IRequestHandler<CreateTopupCommand, object>
{
    private readonly ITopupRepository _repository;
    private readonly IBusPublisher _publisher;
    public CreateTopupCommandHandler(ITopupRepository repository, IBusPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<object> Handle(CreateTopupCommand request, CancellationToken cancellationToken)
    {
        //convert
        try
        {
            var _user = request?.description!.Split("NAPTIEN ")[1].ToLower();     
            var data = Topup.Create(request.id,_user,request.transferAmount);
            await _repository.AddAsync(data);         
            await _publisher.SendAsync<TopupContracts>(new TopupContracts(data.topup_trans_id,data.topup_creator,data.topup_tranfer_amount), cancellationToken);         
            return request;
        }
        catch
        {
            return null!;
        }
       
    }
}
