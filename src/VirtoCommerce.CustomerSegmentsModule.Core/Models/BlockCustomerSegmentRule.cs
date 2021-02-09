using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class BlockCustomerSegmentRule : BlockConditionAndOr
    {
        public BlockCustomerSegmentRule()
        {
            All = true;
        }
    }
}
