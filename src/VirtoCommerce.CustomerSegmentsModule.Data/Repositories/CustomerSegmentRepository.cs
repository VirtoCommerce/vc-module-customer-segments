using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerSegmentsModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Repositories
{
    public class CustomerSegmentRepository : DbContextRepositoryBase<CustomerSegmentDbContext>, ICustomerSegmentRepository
    {
        public CustomerSegmentRepository(CustomerSegmentDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<CustomerSegmentEntity> CustomerSegments => DbContext.Set<CustomerSegmentEntity>();


        public async Task<CustomerSegmentEntity[]> GetByIdsAsync(string[] ids)
        {
            var result = Array.Empty<CustomerSegmentEntity>();

            if (!ids.IsNullOrEmpty())
            {
                result = await CustomerSegments.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            }

            return result;
        }
    }
}
