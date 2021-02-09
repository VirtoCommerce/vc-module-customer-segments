using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CustomerSegmentsModule.Core.Events;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.CustomerSegmentsModule.Core.Services;
using VirtoCommerce.CustomerSegmentsModule.Data.Caching;
using VirtoCommerce.CustomerSegmentsModule.Data.Models;
using VirtoCommerce.CustomerSegmentsModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Services
{
    public class CustomerSegmentService : ICustomerSegmentService
    {
        private readonly Func<ICustomerSegmentRepository> _customerSegmentRepositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IEventPublisher _eventPublisher;

        public CustomerSegmentService(
            Func<ICustomerSegmentRepository> customerSegmentRepositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            IEventPublisher eventPublisher)
        {
            _customerSegmentRepositoryFactory = customerSegmentRepositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _eventPublisher = eventPublisher;
        }

        public virtual async Task<CustomerSegment[]> GetByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", ids.OrderBy(x => x)));

            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                var rules = Array.Empty<CustomerSegment>();

                if (!ids.IsNullOrEmpty())
                {
                    using var customerSegmentsRepository = _customerSegmentRepositoryFactory();

                    //Optimize performance and CPU usage
                    customerSegmentsRepository.DisableChangesTracking();

                    var entities = await customerSegmentsRepository.GetByIdsAsync(ids);

                    rules = entities
                        .Select(x => x.ToModel(AbstractTypeFactory<CustomerSegment>.TryCreateInstance()))
                        .ToArray();

                    cacheEntry.AddExpirationToken(CustomerSegmentCacheRegion.CreateChangeToken(ids));
                }

                return rules;
            });

            return result;
        }

        public virtual async Task SaveChangesAsync(CustomerSegment[] customerSegments)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<CustomerSegment>>();

            using var customerSegmentsRepository = _customerSegmentRepositoryFactory();

            var ids = customerSegments.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray();
            var dbExistProducts = await customerSegmentsRepository.GetByIdsAsync(ids);

            foreach (var customerSegment in customerSegments)
            {
                var modifiedEntity = AbstractTypeFactory<CustomerSegmentEntity>.TryCreateInstance().FromModel(customerSegment, pkMap);
                var originalEntity = dbExistProducts.FirstOrDefault(x => x.Id == customerSegment.Id);

                if (originalEntity != null)
                {
                    changedEntries.Add(new GenericChangedEntry<CustomerSegment>(customerSegment, originalEntity.ToModel(AbstractTypeFactory<CustomerSegment>.TryCreateInstance()), EntryState.Modified));
                    modifiedEntity.Patch(originalEntity);
                }
                else
                {
                    customerSegmentsRepository.Add(modifiedEntity);
                    changedEntries.Add(new GenericChangedEntry<CustomerSegment>(customerSegment, EntryState.Added));
                }
            }

            await _eventPublisher.Publish(new CustomerSegmentChangingEvent(changedEntries));

            await customerSegmentsRepository.UnitOfWork.CommitAsync();
            pkMap.ResolvePrimaryKeys();

            ClearCache(customerSegments);

            await _eventPublisher.Publish(new CustomerSegmentChangedEvent(changedEntries));
        }

        public virtual async Task DeleteAsync(string[] ids)
        {
            var items = await GetByIdsAsync(ids);
            var changedEntries = items
                .Select(x => new GenericChangedEntry<CustomerSegment>(x, EntryState.Deleted))
                .ToArray();

            using var customerSegmentsRepositoryFactory = _customerSegmentRepositoryFactory();

            await _eventPublisher.Publish(new CustomerSegmentChangingEvent(changedEntries));

            var customerSegmentEntities = await customerSegmentsRepositoryFactory.GetByIdsAsync(ids);

            foreach (var customerSegmentEntity in customerSegmentEntities)
            {
                customerSegmentsRepositoryFactory.Remove(customerSegmentEntity);
            }

            await customerSegmentsRepositoryFactory.UnitOfWork.CommitAsync();

            ClearCache(items);

            await _eventPublisher.Publish(new CustomerSegmentChangedEvent(changedEntries));
        }


        protected virtual void ClearCache(IEnumerable<CustomerSegment> customerSegments)
        {
            foreach (var customerSegment in customerSegments)
            {
                CustomerSegmentCacheRegion.ExpireEntity(customerSegment);
            }

            CustomerSegmentSearchCacheRegion.ExpireRegion();
        }
    }
}
