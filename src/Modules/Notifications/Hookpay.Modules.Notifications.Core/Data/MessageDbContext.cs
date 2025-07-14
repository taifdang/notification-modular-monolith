using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Data;

public class MessageDbContext:AppDbContextBase
{
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options) { }
    public DbSet<Message> Message => Set<Message>();
    public DbSet<InboxMessage> InboxMessage => Set<InboxMessage>();
    public DbSet<OutboxMessage> OutboxMessage => Set<OutboxMessage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
