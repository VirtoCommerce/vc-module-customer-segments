using System.Threading.Tasks;
using VirtoCommerce.CustomerSegmentsModule.Core.Models.Search;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Services
{
    public interface ICustomerSegmentSearchService
    {
        Task<CustomerSegmentSearchResult> SearchCustomerSegmentsAsync(CustomerSegmentSearchCriteria criteria);
    }
}
