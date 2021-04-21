using VirtoCommerce.CustomerModule.Core.Model.Search;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public interface IMemberSearchCriteriaBuilder
    {
        MembersSearchCriteria Criteria { get; set; }

        MembersSearchCriteria Build();
    }
}
