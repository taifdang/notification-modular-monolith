using Hookpay.Modules.Topups.Core.Topups.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Dao
{
    public interface ITopupRepository
    {
        Task AddAsync(Topup topup);
    }
}
