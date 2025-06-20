using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.DTO
{
    public class MessagePayload
    {
        public int entity_id { get; set; }
        public string action { get; set; } = default!;
        public string event_type { get; set; } = default!;
        public int user_id { get; set; }
        public Dictionary<string,object> detail { get; set; } = default!;//json tuy bien
        public string? priority { get; set; }// high|medium|low
        //public Users users { get; set; }

    }
}
