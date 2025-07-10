using Hookpay.Modules.Topups.Core.Topups.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Data
{
    public class TopupDbContext:DbContext
    {
        public TopupDbContext(DbContextOptions<TopupDbContext> options) : base(options) { }
        public DbSet<Topup> topup { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        
    }
}
