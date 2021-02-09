using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CustomerModule.Core.Model;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class UserGroupEvaluationContext : IEvaluationContext
    {
        public Contact Customer { get; set; }
    }
}
