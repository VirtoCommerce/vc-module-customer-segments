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
            Criteria.DeepSearch = true;
            Criteria.MemberType = typeof(Contact).Name;
            Criteria.ResponseGroup = MemberResponseGroup.Default.ToString();
        }

        public MembersSearchCriteria Build()
        {
            return Criteria;
        }
    }
}
