using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Generic
{
    public class Email_Details
    {
        public string send_to { get; set; } = default!;
        public string subject { get; set; } = default!;
        public string? body { get; set; }
    }
}
