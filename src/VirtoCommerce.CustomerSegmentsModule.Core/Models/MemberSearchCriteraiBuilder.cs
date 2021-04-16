using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class MemberSearchCriteraiBuilder : IMemberSearchCriteriaBuilder
    {
        public MembersSearchCriteria Criteria { get; set; }

        public MemberSearchCriteraiBuilder()
        {
            Criteria = CreateDefaultCriteria();
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

        protected virtual MembersSearchCriteria CreateDefaultCriteria()
        {
            var criteria = AbstractTypeFactory<MembersSearchCriteria>.TryCreateInstance();
            criteria.MemberType = typeof(Contact).Name;

            return criteria;
        }
    }
}
