using ShareCommon.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace ShareCommon.DTO
{
    public class EventMessageDTO
    {
        public string message_id { get; set; }
        public string? message_type { get; set; } //topupCreated
        public DateTime timestamp { get; set; }
        public string? source { get; set; }
        [JsonConverter(typeof(RawJsonConverter))]
        public string data { get; set; } // data json
        public string? status { get; set; }
        public string? error { get; set; }
    }
}
