using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class InboxNotification
    {
        [Key]
        public int inotify_id { get; set; }
        public Guid inotify_event_id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string inotify_event_type { get; set; } = default!;//[thesieure,sepay]
        [Column(TypeName = "varchar(50)")]
        public string? inotify_source { get; set; } //url
        public string? inotify_payload { get; set; } //json chua tat ca thong tin message tu webhook
        public DateTime inotify_created_at { get; set; }
        public DateTime? inotify_updated_at { get; set; }       
        public string? error { get; set; }
    }
}
