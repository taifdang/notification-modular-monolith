using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Enum
{
    public enum PushType
    {
        // Summary:
        //     push message in web via signalr
        InWeb = 0,
        Email = 1,
        Sms = 2,
    }
}
