using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class MembersSearchCriteriaBuilder : IMemberSearchCriteriaBuilder
    {
        private MembersSearchCriteria Criteria { get; set; }

        public MembersSearchCriteriaBuilder()
        {
            Criteria = AbstractTypeFactory<MembersSearchCriteria>.TryCreateInstance();
            Criteria.MemberType = typeof(Contact).Name;
        }

        public MembersSearchCriteria Build()
        {
            return Criteria;
        }

        public MembersSearchCriteriaBuilder WithPaging(int skip, int take)
        {
            Criteria.Skip = skip;
            Criteria.Take = take;

            return this;
        }

        public MembersSearchCriteriaBuilder WithSearchPhrase(string searchPhrase)
        {
            Criteria.SearchPhrase = $"{Criteria.SearchPhrase} {searchPhrase}".Trim();

            return this;
        }

        public MembersSearchCriteriaBuilder WithSort(string sort)
        {
            Criteria.Sort = sort;

            return this;
        }
    }
}
