using System.Threading.Tasks;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Services
{
    public interface ICustomerSegmentService
    {
        Task<CustomerSegment[]> GetByIdsAsync(string[] ids);

        Task SaveChangesAsync(CustomerSegment[] customerSegments);

        Task DeleteAsync(string[] ids);
    }
}
