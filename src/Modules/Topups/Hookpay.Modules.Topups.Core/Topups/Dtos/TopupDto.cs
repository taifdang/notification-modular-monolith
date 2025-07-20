using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Dtos
{
    public record TopupDto(int id, string accountNumber, string transferType, decimal transferAmount);
}
