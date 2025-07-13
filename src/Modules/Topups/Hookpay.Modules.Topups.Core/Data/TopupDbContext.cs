using Hookpay.Modules.Topups.Core.Topups.Models;
using Hookpay.Shared.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Data
{
    public class TopupDbContext:DbContext
    {
        private readonly IMediator _mediator;
        public TopupDbContext(DbContextOptions<TopupDbContext> options,IMediator mediator) : base(options) { _mediator = mediator; }
        public DbSet<Topup> topup { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var reponse = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await _dispatchDomainEvent();
            return reponse;
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private async Task _dispatchDomainEvent()
        {
            var domainEntites = ChangeTracker.Entries<Entity>()
                .Select(x => x.Entity)
                .Where(x=>x.DomainEvents.Any())
                .ToArray();
            foreach(var domainEvent in domainEntites)
            {
                var events = domainEvent.DomainEvents.ToArray();    
                domainEvent.ClearDomainEvents();
                foreach(var entityDomainEvent in events)
                {
                    await _mediator.Publish(entityDomainEvent);                 
                }
            }
            

        }
        
    }
}
