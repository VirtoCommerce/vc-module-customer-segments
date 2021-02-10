using System.Collections.Generic;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Events
{
    public class CustomerSegmentChangedEvent : GenericChangedEntryEvent<CustomerSegment>
    {
        public CustomerSegmentChangedEvent(IEnumerable<GenericChangedEntry<CustomerSegment>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
