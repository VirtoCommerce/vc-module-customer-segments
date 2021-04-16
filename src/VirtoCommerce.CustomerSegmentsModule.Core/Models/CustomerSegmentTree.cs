using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.Common;

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
            if (!Children.IsNullOrEmpty())
            {
                BuildSearchCriteria(Children, builder);
            }

            return builder.Build();
        }

        protected virtual void BuildSearchCriteria(IList<IConditionTree> children, IMemberSearchCriteriaBuilder builder)
        {
            foreach (var conditionTree in children)
            {
                BuildSearchCriteria(conditionTree.Children, builder);

                if (conditionTree is ICanBuildSearchCriteria buildableCondition)
                {
                    _ = buildableCondition.BuildSearchCriteria(builder);
                }
            }
        }
    }
}
