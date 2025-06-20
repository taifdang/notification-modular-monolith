using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Enum
{
    public enum MessagePriority//seconds
    {    
        High = 0,//imediate
        Medium = 5,//deplay 5 second
        Low = 10,//deplay 10 second
    }
}
