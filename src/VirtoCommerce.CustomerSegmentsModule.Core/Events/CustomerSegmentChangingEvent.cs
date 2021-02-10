using System.Collections.Generic;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Events
{
    public class CustomerSegmentChangingEvent : GenericChangedEntryEvent<CustomerSegment>
    {
        public CustomerSegmentChangingEvent(IEnumerable<GenericChangedEntry<CustomerSegment>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
