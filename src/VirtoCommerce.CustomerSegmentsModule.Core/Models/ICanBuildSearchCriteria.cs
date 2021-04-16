using VirtoCommerce.CustomerModule.Core.Model.Search;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public interface ICanBuildSearchCriteria
    {
        MembersSearchCriteria BuildSearchCriteria(IMemberSearchCriteriaBuilder builder);
    }
}
