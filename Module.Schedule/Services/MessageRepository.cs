using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Schedule.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DatabaseContext _db;
        public MessageRepository(DatabaseContext db)
        {
            _db = db;
        }
        public void Dispose()
        {
            _db?.Dispose();
        }

        public IEnumerable<string?> Filter(List<Messages> messages)
        {
            var filter_list = messages
                .Join(_db.users,
                   message_tbl => message_tbl.mess_user_id,
                   user_tbl => user_tbl.user_id,
                   (message_tbl, user_tbl) => new { message_tbl, user_tbl }
                )
                .Join(_db.settings,
                   dispatch => dispatch.user_tbl.user_id,
                   setting_tbl => setting_tbl.set_user_id,
                   (dispatch, setting_tbl) => new {dispatch.message_tbl,dispatch.user_tbl,setting_tbl}
                )
                .Where(x=>x.setting_tbl.disable_notification == false && x.user_tbl.is_block == false)
                .Select(item=>item.message_tbl.mess_payload)
                .ToList();
                
            return filter_list;
        }       

        public async Task<List<Messages>> GetMessagesAsync()
        {
            return await _db.messages.ToListAsync();
        }
    }
}
