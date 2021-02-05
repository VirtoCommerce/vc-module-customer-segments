using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Repositories
{
    public class VirtoCommerceCustomerSegmentsModuleDbContext : DbContextWithTriggers
    {
        public VirtoCommerceCustomerSegmentsModuleDbContext(DbContextOptions<VirtoCommerceCustomerSegmentsModuleDbContext> options)
          : base(options)
        {
        }

        protected VirtoCommerceCustomerSegmentsModuleDbContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //        modelBuilder.Entity<MyModuleEntity>().ToTable("MyModule").HasKey(x => x.Id);
            //        modelBuilder.Entity<MyModuleEntity>().Property(x => x.Id).HasMaxLength(128);
            //        base.OnModelCreating(modelBuilder);
        }
    }
}

