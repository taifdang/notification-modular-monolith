using Microsoft.EntityFrameworkCore;
using Hookpay.Shared.EFCore;
namespace Hookpay.Modules.Users.Core.Data
{
    public class UserDbContext:AppDbContextBase
    {
        public UserDbContext(DbContextOptions<UserDbContext> options):base(options) { }
        public DbSet<Users.Models.UserSetting> UserSetting => Set<Users.Models.UserSetting>();
        public DbSet<Users.Models.Users> Users => Set<Users.Models.Users>();    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
