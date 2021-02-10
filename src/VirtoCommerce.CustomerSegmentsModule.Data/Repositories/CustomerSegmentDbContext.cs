using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerSegmentsModule.Data.Models;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Repositories
{
    public class CustomerSegmentDbContext : DbContextWithTriggers
    {
        public CustomerSegmentDbContext(DbContextOptions<CustomerSegmentDbContext> options)
          : base(options)
        {
        }

        protected CustomerSegmentDbContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerSegmentEntity>().ToTable("CustomerSegments").HasKey(x => x.Id);
            modelBuilder.Entity<CustomerSegmentEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}

