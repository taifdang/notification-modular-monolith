using ShareCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.DTO
{
    public class NotifyPayload {
        public PushType push_type { get;set; }
        public string? send_to { get; set; }
        public string? subject { get; set; }
        public string? body { get; set; } = null;
 
    }

}
