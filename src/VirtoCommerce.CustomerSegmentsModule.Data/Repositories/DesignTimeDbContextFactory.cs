using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CustomerSegmentDbContext>
    {
        public CustomerSegmentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CustomerSegmentDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new CustomerSegmentDbContext(builder.Options);
        }
    }
}
