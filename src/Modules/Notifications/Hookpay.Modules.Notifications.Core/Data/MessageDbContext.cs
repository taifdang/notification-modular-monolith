using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.EFCore;
using Microsoft.EntityFrameworkCore;


namespace Hookpay.Modules.Notifications.Core.Data;

public class MessageDbContext:AppDbContextBase
{
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options) { }
    public DbSet<Message> Message => Set<Message>();
    //public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    //public DbSet<OutboxState> OutboxState => Set<OutboxState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.AddOutboxMessageEntity();
        //modelBuilder.AddOutboxStateEntity();
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
