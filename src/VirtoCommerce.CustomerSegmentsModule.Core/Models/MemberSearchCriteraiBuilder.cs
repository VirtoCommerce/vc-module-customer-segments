using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class MemberSearchCriteraiBuilder : IMemberSearchCriteriaBuilder
    {
        private MembersSearchCriteria Criteria { get; set; }

        public MemberSearchCriteraiBuilder()
        {
            Criteria = AbstractTypeFactory<MembersSearchCriteria>.TryCreateInstance();
            Criteria.MemberType = typeof(Contact).Name;
        }

        public MembersSearchCriteria Build()
        {
            return Criteria;
        }

        public MemberSearchCriteraiBuilder WithPaging(int skip, int take)
        {
            Criteria.Skip = skip;
            Criteria.Take = take;

            return this;
        }

        public MemberSearchCriteraiBuilder WithSearchPhrase(string searchPhrase)
        {
            Criteria.SearchPhrase = $"{Criteria.SearchPhrase} {searchPhrase}".Trim();

            return this;
        }

        public MemberSearchCriteraiBuilder WithSort(string sort)
        {
            Criteria.Sort = sort;

            return this;
        }
    }
}
