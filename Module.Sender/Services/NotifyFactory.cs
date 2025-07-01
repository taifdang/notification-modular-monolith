using ShareCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Sender.Services
{
    public class NotifyFactory : INotifyFactory
    {
        private readonly IEnumerable<INotifyStrategy> _strategy;
        public NotifyFactory(IEnumerable<INotifyStrategy> strategy)
        {
            _strategy = strategy;
        }
        public INotifyStrategy GetStrategy(PushType pushType)
        {
            var strategy = _strategy.FirstOrDefault(x => x.GetPushType == pushType);
            return strategy ?? throw new NotSupportedException();
        }
    }
}
