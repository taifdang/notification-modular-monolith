using ShareCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Sender.Services
{
    public interface INotifyFactory
    {
        INotifyStrategy GetStrategy(PushType pushType);
    }
}
