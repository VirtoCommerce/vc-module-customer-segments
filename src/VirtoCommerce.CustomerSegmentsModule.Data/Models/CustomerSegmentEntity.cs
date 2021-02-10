using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.CustomerSegmentsModule.Data.JsonConverters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerSegmentsModule.Data.Models
{
    public class CustomerSegmentEntity : AuditableEntity
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        [StringLength(64)]
        public string UserGroup { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string SerializedExpressionTree { get; set; }

        public virtual CustomerSegment ToModel(CustomerSegment customerSegment)
        {
            if (customerSegment == null)
            {
                throw new ArgumentNullException(nameof(customerSegment));
            }

            customerSegment.Id = Id;
            customerSegment.CreatedBy = CreatedBy;
            customerSegment.CreatedDate = CreatedDate;
            customerSegment.ModifiedBy = ModifiedBy;
            customerSegment.ModifiedDate = ModifiedDate;

            customerSegment.Name = Name;
            customerSegment.Description = Description;
            customerSegment.UserGroup = UserGroup;
            customerSegment.IsActive = IsActive;
            customerSegment.StartDate = StartDate;
            customerSegment.EndDate = EndDate;

            customerSegment.ExpressionTree = AbstractTypeFactory<CustomerSegmentTree>.TryCreateInstance();
            if (SerializedExpressionTree != null)
            {
                customerSegment.ExpressionTree = JsonConvert.DeserializeObject<CustomerSegmentTree>(SerializedExpressionTree, new CustomerSegmentConditionJsonConverter());
            }

            return customerSegment;
        }

        public virtual CustomerSegmentEntity FromModel(CustomerSegment customerSegment, PrimaryKeyResolvingMap pkMap)
        {
            if (customerSegment == null)
            {
                throw new ArgumentNullException(nameof(customerSegment));
            }

            pkMap.AddPair(customerSegment, this);

            Id = customerSegment.Id;
            CreatedBy = customerSegment.CreatedBy;
            CreatedDate = customerSegment.CreatedDate;
            ModifiedBy = customerSegment.ModifiedBy;
            ModifiedDate = customerSegment.ModifiedDate;

            Name = customerSegment.Name;
            Description = customerSegment.Description;
            UserGroup = customerSegment.UserGroup;
            IsActive = customerSegment.IsActive;
            StartDate = customerSegment.StartDate;
            EndDate = customerSegment.EndDate;

            if (customerSegment.ExpressionTree != null)
            {
                SerializedExpressionTree = JsonConvert.SerializeObject(customerSegment.ExpressionTree, new CustomerSegmentConditionJsonConverter(doNotSerializeAvailCondition: true));
            }

            return this;
        }

        public virtual void Patch(CustomerSegmentEntity target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            target.Name = Name;
            target.Description = Description;
            target.UserGroup = UserGroup;
            target.IsActive = IsActive;
            target.StartDate = StartDate;
            target.EndDate = EndDate;
            target.SerializedExpressionTree = SerializedExpressionTree;
        }
    }
}
