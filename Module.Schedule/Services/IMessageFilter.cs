using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Schedule.Services
{
    public interface IMessageFilter
    {
        IEnumerable<string?> Filter(List<Messages> messages);
    }
}
