using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Modules.Topups.Core.Topups.Dao;
using Hookpay.Modules.Topups.Core.Topups.Models;
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
    public CreateTopupCommandHandler(ITopupRepository repository)
    {
        _repository = repository;
    }

    public async Task<object> Handle(CreateTopupCommand request, CancellationToken cancellationToken)
    {
        //convert
        try
        {
            var _user = request?.description!.Split("NAPTIEN ")[1].ToLower();
            var data = new Topup
            {
                topup_trans_id = request.id,
                topup_creator = _user,
                topup_tranfer_amount = request.transferAmount,
                topup_created_at = DateTime.UtcNow,
            };
            await _repository.AddAsync(data);
            return request;
        }
        catch
        {
            return null!;
        }
       
    }
}
