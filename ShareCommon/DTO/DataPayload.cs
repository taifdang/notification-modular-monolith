using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.DTO
{
    public class DataPayload<T>
    {
        public int entity_id { get; set; }
        public string action { get; set; } = default!;
        public string status { get; set; } = default!;
        public int user_id { get; set; }
        public T detail { get; set; } = default!;//json tuy bien
        public string? priority { get; set; }// high|medium|low

    }
}
