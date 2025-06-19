using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Enum
{
    public enum MessageStatus
    {
        Pending = 0,//prepare send message
        Processed = 1,//sucecss
        Failed = 2,//fail      
    }
}
