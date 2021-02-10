using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.CustomerSegmentsModule.Core.Models.Search;
using VirtoCommerce.CustomerSegmentsModule.Core.Services;
using VirtoCommerce.CustomerSegmentsModule.Data.Caching;
using VirtoCommerce.CustomerSegmentsModule.Data.Models;
using VirtoCommerce.CustomerSegmentsModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Services
{
    public class CustomerSegmentSearchService : ICustomerSegmentSearchService
    {
        private readonly Func<ICustomerSegmentRepository> _customerSegmentRepositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly ICustomerSegmentService _customerSegmentService;

        public CustomerSegmentSearchService(Func<ICustomerSegmentRepository> customerSegmentRepositoryFactory, IPlatformMemoryCache platformMemoryCache, ICustomerSegmentService customerSegmentService)
        {
            _customerSegmentRepositoryFactory = customerSegmentRepositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _customerSegmentService = customerSegmentService;
        }

        public virtual async Task<CustomerSegmentSearchResult> SearchCustomerSegmentsAsync(CustomerSegmentSearchCriteria criteria)
        {
            ValidateParameters(criteria);

            var cacheKey = CacheKey.With(GetType(), nameof(SearchCustomerSegmentsAsync), criteria.GetCacheKey());

            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AddExpirationToken(CustomerSegmentSearchCacheRegion.CreateChangeToken());

                var result = AbstractTypeFactory<CustomerSegmentSearchResult>.TryCreateInstance();

                using (var customerSegmentsRepositoryFactory = _customerSegmentRepositoryFactory())
                {
                    //Optimize performance and CPU usage
                    customerSegmentsRepositoryFactory.DisableChangesTracking();

                    var sortInfos = BuildSortExpression(criteria);
                    var query = BuildQuery(customerSegmentsRepositoryFactory, criteria);

                    result.TotalCount = await query.CountAsync();

                    if (criteria.Take > 0 && result.TotalCount > 0)
                    {
                        var ids = await query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id)
                            .Select(x => x.Id)
                            .Skip(criteria.Skip).Take(criteria.Take)
                            .AsNoTracking()
                            .ToArrayAsync();

                        result.Results = (await _customerSegmentService.GetByIdsAsync(ids)).OrderBy(x => Array.IndexOf(ids, x.Id)).ToList();
                    }
                }

                return result;
            });
        }

        protected virtual IQueryable<CustomerSegmentEntity> BuildQuery(ICustomerSegmentRepository repository, CustomerSegmentSearchCriteria criteria)
        {
            var query = repository.CustomerSegments;

            if (!string.IsNullOrEmpty(criteria.Keyword))
            {
                query = query.Where(x => x.Name.Contains(criteria.Keyword));
            }

            if (criteria.IsActive != null)
            {
                var utcNow = DateTime.UtcNow;
                query = query.Where(x => x.IsActive == criteria.IsActive && (x.StartDate == null || utcNow >= x.StartDate) && (x.EndDate == null || x.EndDate >= utcNow));
            }

            return query;
        }

        protected virtual IList<SortInfo> BuildSortExpression(CustomerSegmentSearchCriteria criteria)
        {
            var sortInfos = criteria.SortInfos;

            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[] { new SortInfo { SortColumn = nameof(CustomerSegment.Name) } };
            }

            return sortInfos;
        }

        private static void ValidateParameters(CustomerSegmentSearchCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
        }
    }
}
