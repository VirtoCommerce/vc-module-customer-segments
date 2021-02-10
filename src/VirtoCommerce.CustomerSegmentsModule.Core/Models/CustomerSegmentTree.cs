using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegmentTree : BlockConditionAndOr
    {
        public CustomerSegmentTree()
        {
            All = true;
        }
    }
}
