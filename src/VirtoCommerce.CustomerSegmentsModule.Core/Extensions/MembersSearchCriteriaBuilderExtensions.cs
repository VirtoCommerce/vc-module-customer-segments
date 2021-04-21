using VirtoCommerce.CustomerSegmentsModule.Core.Models;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Extensions
{
    public static class MembersSearchCriteriaBuilderExtensions
    {
        public static IMemberSearchCriteriaBuilder WithPaging(this IMemberSearchCriteriaBuilder builder, int skip, int take)
        {
            builder.Criteria.Skip = skip;
            builder.Criteria.Take = take;

            return builder;
        }

        public static IMemberSearchCriteriaBuilder WithSearchPhrase(this IMemberSearchCriteriaBuilder builder, string searchPhrase)
        {
            builder.Criteria.SearchPhrase = $"{builder.Criteria.SearchPhrase} {searchPhrase}".Trim();

            return builder;
        }

        public static IMemberSearchCriteriaBuilder WithSort(this IMemberSearchCriteriaBuilder builder, string sort)
        {
            builder.Criteria.Sort = sort;

            return builder;
        }
    }
}
