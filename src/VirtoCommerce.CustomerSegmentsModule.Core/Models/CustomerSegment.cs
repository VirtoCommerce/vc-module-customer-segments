using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegment : AuditableEntity
    {
        public CustomerSegment()
        {
            StoreIds = new List<string>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string UserGroup { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<string> StoreIds { get; set; }

        public CustomerSegmentTree ExpressionTree { get; set; }
    }
}
