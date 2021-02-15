using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegmentTreePrototype : ConditionTree
    {
        public CustomerSegmentTreePrototype()
        {
            var rule = new BlockCustomerSegmentRule()
                .WithAvailConditions(new CustomerSegmentConditionPropertyValues());

            WithAvailConditions(rule);
            WithChildrens(rule);
        }
    }
}
