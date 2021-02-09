using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class ConditionPropertyValues : ConditionTree
    {
        public ICollection<DynamicObjectProperty> Properties { get; set; } = new List<DynamicObjectProperty>();

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            var result = false;

            if (context is CustomerSegmentExpressionEvaluationContext evaluationContext)
            {
                return evaluationContext.CustomerHasPropertyValues(Properties);
            }

            return result;
        }
    }
}
