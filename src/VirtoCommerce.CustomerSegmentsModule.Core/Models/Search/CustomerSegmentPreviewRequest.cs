using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models.Search
{
    public class CustomerSegmentPreviewRequest : SearchCriteriaBase
    {
        public CustomerSegmentTree Expression { get; set; }
    }
}
