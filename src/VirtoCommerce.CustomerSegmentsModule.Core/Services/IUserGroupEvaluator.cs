using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Services
{
    public interface IUserGroupEvaluator
    {
        Task<ICollection<UserGroupInfo>> EvaluateUserGroupsAsync(IEvaluationContext context);
        ICollection<UserGroupInfo> EvaluateUserGroups(IEvaluationContext context);
    }
}
