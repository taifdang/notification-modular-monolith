using Hookpay.Modules.Topups.Core.Topups.Models;
using Hookpay.Shared.Domain.Models;
using Hookpay.Shared.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Topups.Core.Data
{
    public class TopupDbContext:AppDbContextBase
    {
        private readonly IMediator _mediator;
        public TopupDbContext(DbContextOptions<TopupDbContext> options,IMediator mediator) : base(options) { _mediator = mediator; }
        public DbSet<Topup> Topup => Set<Topup>();  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }     
    }
}
