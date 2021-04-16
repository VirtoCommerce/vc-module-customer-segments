using VirtoCommerce.CustomerModule.Core.Model.Search;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public interface IMemberSearchCriteriaBuilder
    {
        MembersSearchCriteria Build();

        MemberSearchCriteraiBuilder WithPaging(int skip, int take);

        MemberSearchCriteraiBuilder WithSearchPhrase(string searchPhrase);

        MemberSearchCriteraiBuilder WithSort(string sort);
    }
}
