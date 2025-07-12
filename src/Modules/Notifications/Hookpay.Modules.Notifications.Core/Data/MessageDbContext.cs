using Hookpay.Modules.Notifications.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Data;

public class MessageDbContext:DbContext
{
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options) { }
    public DbSet<Message> message { get; set; }
    public DbSet<InboxMessage> inboxMessage { get; set; }
    public DbSet<OutboxMessage> outboxMessage { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
