using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class MembersSearchCriteriaBuilder : IMemberSearchCriteriaBuilder
    {
        public MembersSearchCriteria Criteria { get; set; }

        public MembersSearchCriteriaBuilder()
        {
            Criteria = AbstractTypeFactory<MembersSearchCriteria>.TryCreateInstance();
            Criteria.MemberType = typeof(Contact).Name;
        }

        public MembersSearchCriteria Build()
        {
            return Criteria;
        }
    }
}
