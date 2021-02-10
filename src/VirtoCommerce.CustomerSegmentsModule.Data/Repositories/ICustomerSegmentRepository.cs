using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CustomerSegmentsModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Repositories
{
    public interface ICustomerSegmentRepository : IRepository
    {
        IQueryable<CustomerSegmentEntity> CustomerSegments { get; }

        Task<CustomerSegmentEntity[]> GetByIdsAsync(string[] ids);
    }
}
