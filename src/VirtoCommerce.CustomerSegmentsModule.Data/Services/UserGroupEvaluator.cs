using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.CustomerSegmentsModule.Core.Models.Search;
using VirtoCommerce.CustomerSegmentsModule.Core.Services;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Services
{
    public class UserGroupEvaluator : IUserGroupEvaluator
    {
        private readonly ICustomerSegmentSearchService _customerSegmentSearchService;

        public UserGroupEvaluator(ICustomerSegmentSearchService customerSegmentSearchService)
        {
            _customerSegmentSearchService = customerSegmentSearchService;
        }

        public async Task<ICollection<string>> EvaluateUserGroupsAsync(IEvaluationContext context)
        {
            var result = Array.Empty<string>();

            if (context is UserGroupEvaluationContext evaluationContext)
            {
                var customerSegments =
                    (await _customerSegmentSearchService.SearchCustomerSegmentsAsync(new CustomerSegmentSearchCriteria { IsActive = true })).Results;

                result = customerSegments
                    .Where(customerSegment => customerSegment.ExpressionTree.IsSatisfiedBy(new CustomerSegmentExpressionEvaluationContext { Customer = evaluationContext.Customer }))
                    .Select(customerSegment => customerSegment.UserGroup)
                    .ToArray();
            }

            return result;
        }

        public ICollection<string> EvaluateUserGroups(IEvaluationContext context)
        {
            return EvaluateUserGroupsAsync(context).GetAwaiter().GetResult();
        }
    }
}
