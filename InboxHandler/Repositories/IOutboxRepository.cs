using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Topup.Repositories
{
    public interface IOutboxRepository: IRepository<OutboxTopup>
    {
        Task AddToOutBox(OutboxTopup outbox);
    }
}
