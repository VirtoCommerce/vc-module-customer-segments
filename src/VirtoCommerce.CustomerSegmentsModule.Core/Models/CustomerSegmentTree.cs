using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CustomerModule.Core.Model.Search;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegmentTree : BlockConditionAndOr, ICanBuildSearchCriteria
    {
        public CustomerSegmentTree()
        {
            All = true;
        }

        public virtual MembersSearchCriteria BuildSearchCriteria(IMemberSearchCriteriaBuilder builder)
        {
            EvaluateSearchCriteria(Children, builder);

            return builder.Build();
        }

        protected virtual void EvaluateSearchCriteria(IList<IConditionTree> children, IMemberSearchCriteriaBuilder builder)
        {
            if (children != null)
            {
                foreach (var conditionTree in children)
                {
                    EvaluateSearchCriteria(conditionTree.Children, builder);

                    if (conditionTree is ICanBuildSearchCriteria buildableCondition)
                    {
                        _ = buildableCondition.BuildSearchCriteria(builder);
                    }
                }
            }
        }
    }
}
