using Microsoft.EntityFrameworkCore;
using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<InboxNotification> inbox_notification { get; set; }
        public DbSet<InboxTopup> inbox_topup { get; set; }
        public DbSet<Topup> topup { get; set; }
        public DbSet<OutboxTopup> outbox_topup { get; set; }
        public DbSet<Users> users { get; set; }
        public DbSet<Messages> messages { get; set; }
        public DbSet<HubConnection> hub_connection { get; set; }
        public DbSet<Settings> settings { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //user-message (1-n)
            modelBuilder.Entity<Messages>().HasOne(x=>x.users).WithMany(y=>y.messages).HasForeignKey(z=>z.mess_user_id);
            //user-setting (1-1)
            modelBuilder.Entity<Users>().HasOne(x => x.settings).WithOne(y => y.users).HasForeignKey<Settings>(e => e.set_user_id);
                
        }
    }
}
