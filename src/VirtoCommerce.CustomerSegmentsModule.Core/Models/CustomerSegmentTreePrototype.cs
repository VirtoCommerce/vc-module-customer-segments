using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegmentTreePrototype : ConditionTree
    {
        public CustomerSegmentTreePrototype()
        {
            var rule = new BlockCustomerSegmentRule()
                .WithAvailConditions(new ConditionPropertyValues());

            WithAvailConditions(rule);
            WithChildrens(rule);
        }
    }
}
