using Microsoft.EntityFrameworkCore;
using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<InboxTopup> inbox_topup { get; set; }
        public DbSet<Transactions> transactions { get; set; }
        public DbSet<OutboxTopup> outbox_topup { get; set; }
        public DbSet<Users> users { get; set; }
    }
}
