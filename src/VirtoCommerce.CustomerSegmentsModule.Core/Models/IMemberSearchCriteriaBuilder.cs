using VirtoCommerce.CustomerModule.Core.Model.Search;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public interface IMemberSearchCriteriaBuilder
    {
        MembersSearchCriteria Build();

        MembersSearchCriteriaBuilder WithPaging(int skip, int take);

        MembersSearchCriteriaBuilder WithSearchPhrase(string searchPhrase);

        MembersSearchCriteriaBuilder WithSort(string sort);
    }
}
