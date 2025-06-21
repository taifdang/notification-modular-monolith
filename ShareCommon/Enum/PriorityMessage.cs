using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Enum
{
    public enum PriorityMessage
    {    
        Low = 0,//imediate
        Medium = 1,//delay
        High = 2,//await
    }
}
